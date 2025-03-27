using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using todo_app_backend.Data;
using todo_app_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using todo_app_backend.DTOs.Auth;
using todo_app_backend.Helpers;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace todo_app_backend.Repositories
{
    public class AuthRepository(AppDbContext appDbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IAuthRepository
    {
        public async Task<bool> FindAnyByIdAsync(string userId)
        {
            return await appDbContext.User.AnyAsync(user => user.Id == userId);
        }

        public async Task<bool> CheckUserActiveAsync(string email)
        {
            var user = await appDbContext.User.FirstOrDefaultAsync(user => user.Email == email);

            if (user is null)
            {
                return false;
            }

            return user.IsActive;
        }

        public async Task<string> ValidateOtp(string userEmail, string otpText)
        {
            var otp = await appDbContext.Otp.FirstOrDefaultAsync(otp => otp.UserEmail == userEmail && otp.Text == otpText);

            if (otp is null)
            {
                return "wrong";
            }
            else
            {
                if (otp.ExpiryTime <= DateTime.Now)
                {
                    return "expired";
                }
                return "success";
            }
        }

        //
        public async Task<string> GetOAuthAccessTokenAsync()
        {
            var credential = await OAuth.GetCredentialAsync();
            return credential.Token.AccessToken;
        }

        public async Task<string> GetOAuthRefreshTokenAsync()
        {
            var credential = await OAuth.GetCredentialAsync();
            return credential.Token.RefreshToken;
        }

        //
        public async Task SendEmailAsync(string otpText, string firstName, string emailReceiver)
        {
            string accessToken = await GetOAuthAccessTokenAsync();
            Console.WriteLine($"Access Token: {accessToken}");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Todo Task App", "todotaskapp.mail@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailReceiver));
            email.Subject = "Email Confirmation";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"<div>
                            <div>Hi {firstName},</div>
                            <br/>
                            <div>We got your request to confirm this email for our Todo Task App.
                            Here's the code to verify your email in Todo Task App:</div>
                            <br/>
                            <div align='center'>
                              <div align='center' style='letter-spacing: 7px; height: 30px; width: 300px; 
                                  padding: 15px; background-color: #dcdcfe; border: 2px solid #1F51FF; 
                                  border-radius: 10px; font-weight: bold; font-size: 20px; align-items: center;'>
                                  {otpText}
                              </div>
                            </div>
                            <br/>
                            <div style='color: grey;' align='center'>
                                Please do not share this code with anyone.
                            </div>
                            <br/>
                            <div>Thanks,</div>
                            <div>Todo Task App</div>
                        </div>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            var oauth2 = new SaslMechanismOAuth2("todotaskapp.mail@gmail.com", accessToken);
            await smtp.AuthenticateAsync(oauth2);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task<APIResponse?> RegisterAsync(string userEmail, string otpText)
        {
            APIResponse response = new APIResponse();
            string otpValidateMessage = await ValidateOtp(userEmail, otpText ?? "");

            if (otpValidateMessage == "success")
            {
                // convert Temp User to User
                var tempUser = await appDbContext.TempUser.FirstOrDefaultAsync(user => user.Email == userEmail);
                var user = new User();

                if (tempUser is not null)
                {
                    user.Id = tempUser.Id;
                    user.FirstName = tempUser.FirstName;
                    user.LastName = tempUser.LastName;
                    user.Username = tempUser.Username;
                    user.Email = tempUser.Email;
                    user.Phone = tempUser.Phone;
                    user.Password = tempUser.Password;
                    user.IsActive = tempUser.IsActive;
                    user.CreatedAt = tempUser.CreatedAt;

                    await appDbContext.User.AddAsync(user);

                    // delete Temp User
                    appDbContext.TempUser.Remove(tempUser);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Temp User does not exist!";

                    return response;
                }

                await appDbContext.SaveChangesAsync();

                return response = new APIResponse()
                {
                    Success = true,
                    Message = "Register successfully!",
                    Data = user
                };
            }
            else if (otpValidateMessage == "wrong")
            {
                response.Success = false;
                response.Message = "Wrong OTP!";

                return response;
            }
            else
            {
                response.Success = false;
                response.Message = "OTP is expired!";

                return response;
            }
        }

        private string GenerateOtp()
        {
            Random randomNumber = new Random();
            string randomNumberGenerator = randomNumber.Next(0, 1000000).ToString("D6");
            return randomNumberGenerator;
        }

        public async Task<APIResponse?> AddOrUpdateOtp(UserRegisterDto userRegisterDto)
        {
            APIResponse response = new APIResponse();
            var otp = await appDbContext.Otp.FirstOrDefaultAsync(otp => otp.UserEmail == userRegisterDto.Email);

            if (otp is null)
            {
                otp = new Otp()
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = GenerateOtp(),
                    ExpiryTime = DateTime.Now.AddMinutes(5),
                    CreatedAt = DateTime.Now,
                    UserEmail = userRegisterDto.Email
                };

                await appDbContext.Otp.AddAsync(otp);
            }
            else
            {
                otp.Id = otp.Id;
                otp.Text = GenerateOtp();
                otp.ExpiryTime = DateTime.Now.AddMinutes(5);
                otp.CreatedAt = DateTime.Now;
                otp.UserEmail = otp.UserEmail;
            }

            await SendEmailAsync(otp.Text, userRegisterDto.FirstName, otp.UserEmail);

            // Add Temp User
            if (await appDbContext.TempUser.AnyAsync(user => user.Email == userRegisterDto.Email)
                || await appDbContext.User.AnyAsync(user => user.Email == userRegisterDto.Email))
            {
                response.Success = false;
                response.Message = "User is already existed!";

                return response;
            }

            var tempUser = new TempUser();
            var hashedPassword = new PasswordHasher<TempUser>().HashPassword(tempUser, userRegisterDto.Password);

            tempUser.Id = Guid.NewGuid().ToString();
            tempUser.FirstName = userRegisterDto.FirstName;
            tempUser.LastName = userRegisterDto.LastName;
            tempUser.Username = userRegisterDto.Username;
            tempUser.Email = userRegisterDto.Email;
            tempUser.Phone = userRegisterDto.Phone;
            tempUser.Password = hashedPassword;
            tempUser.IsActive = true;
            tempUser.CreatedAt = DateTime.UtcNow;

            await appDbContext.TempUser.AddAsync(tempUser);

            await appDbContext.SaveChangesAsync();

            return response = new APIResponse()
            {
                Success = true,
                Message = "Update OTP successfully!"
            };
        }

        public async Task<APIResponse?> ResendOtp(string userEmail, string userFirstName)
        {
            if (await appDbContext.TempUser.AnyAsync(user => user.Email == userEmail))
            {
                // update OTP
                var otp = await appDbContext.Otp.FirstOrDefaultAsync(otp => otp.UserEmail == userEmail);

                if (otp is not null)
                {
                    otp.Id = otp.Id;
                    otp.Text = GenerateOtp();
                    otp.ExpiryTime = DateTime.Now.AddMinutes(5);
                    otp.CreatedAt = DateTime.Now;
                    otp.UserEmail = otp.UserEmail;

                    await appDbContext.SaveChangesAsync();

                    await SendEmailAsync(otp.Text, userFirstName, userEmail);
                }

                return new APIResponse()
                {
                    Success = true,
                    Message = "User email confirmation request is resent!"
                };

            }
            else
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "User email confirmation request does not exist!"
                };
            }

        }

        public async Task<string?> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await appDbContext.User.FirstOrDefaultAsync(user => user.Email == userLoginDto.Email);

            if (user is null)
            {
                return null;
            }

            var verifiedHashedPassword = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, userLoginDto.Password);
            if (verifiedHashedPassword == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var tokens = await CreateTokenResponse(user);
            return tokens;
        }

        public async Task<string?> CreateTokenResponse(User user)
        {
            string accessToken = CreateToken(user!);
            string refreshToken = await GenerateAndSaveRefreshTokenAsync(user!);

            // Store refresh token in HttpOnly Secure Cookie
            httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Only for HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7) // Refresh token expiry
            });

            return accessToken;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var JwtSetting = configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtSetting.GetSection("SecurityKey").Value!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: JwtSetting.GetSection("Issuer").Value!,
                audience: JwtSetting.GetSection("Audience").Value!,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await appDbContext.SaveChangesAsync();
            return refreshToken;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<User?> ValidateRefreshTokenAsync(string userId)
        {
            if (!httpContextAccessor.HttpContext!.Request.Cookies.TryGetValue("refreshToken", out var refreshToken)) {
                return null;
            }

            var user = await appDbContext.User.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }

        public async Task<string?> RefreshTokenAsync(UserRefreshTokenDto userRefreshTokenDto)
        {
            var user = await ValidateRefreshTokenAsync(userRefreshTokenDto.UserId!);

            if (user is null)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        public async Task<UserInfoDto?> GetByEmailAsync(string email)
        {
            var user = await appDbContext.User.FirstOrDefaultAsync(user => user.Email == email);

            if (user is null)
            {
                return null;
            }

            return new UserInfoDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive
            };
        }

        public async Task<UserInfoDto?> UpdateAsync(UserInfoDto userInfoDto)
        {
            var user = await appDbContext.User.FirstOrDefaultAsync(user => user.Id == userInfoDto.Id);

            if (user is null)
            {
                return null;
            }

            user.FirstName = userInfoDto.FirstName ?? user.FirstName;
            user.LastName = userInfoDto.LastName ?? user.LastName;
            user.Username = userInfoDto.Username ?? user.Username;
            user.Email = userInfoDto.Email ?? user.Email;
            user.Phone = userInfoDto.Phone ?? user.Phone;
            user.IsActive = userInfoDto.IsActive ?? user.IsActive;

            await appDbContext.SaveChangesAsync();

            return new UserInfoDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive
            };
        }
    }
}