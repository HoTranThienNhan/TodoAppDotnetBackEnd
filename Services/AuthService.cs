using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using todo_app_backend.DTOs.Auth;
using todo_app_backend.Helpers;
using todo_app_backend.Models;
using todo_app_backend.Repositories.Contracts;
using todo_app_backend.Services.Contracts;

namespace todo_app_backend.Services
{
    public class AuthService(IAuthRepository authRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        public async Task<APIResponse?> RegisterAsync(string userEmail, string otpText)
        {
            APIResponse response = new APIResponse();
            string otpValidateMessage = await ValidateOtp(userEmail, otpText ?? "");

            if (otpValidateMessage == "success")
            {
                // convert Temp User to User
                var tempUser = await authRepository.GetTempUserByEmail(userEmail);
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
                    user.Avatar = tempUser.Avatar;
                    user.IsActive = tempUser.IsActive;
                    user.CreatedAt = tempUser.CreatedAt;

                    await authRepository.AddUserAsync(user);

                    // delete Temp User
                    await authRepository.DeleteTempUser(tempUser);

                }
                else
                {
                    response.Success = false;
                    response.Message = "Temp User does not exist!";

                    return response;
                }

                return new APIResponse()
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

        public async Task<string> ValidateOtp(string userEmail, string otpText)
        {
            var otp = await authRepository.GetOtpByEmailAndTextAsync(userEmail, otpText);

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

        public async Task<APIResponse?> AddOrUpdateOtp(UserRegisterDto userRegisterDto)
        {
            APIResponse response = new APIResponse();
            // Add Temp User
            if (await authRepository.FindAnyTempUserByEmailAsync(userRegisterDto.Email)
                || await authRepository.FindAnyByEmailAsync(userRegisterDto.Email))
            {
                response.Success = false;
                response.Message = "User is already existed!";

                return response;
            }

            var otp = await authRepository.GetOtpByEmailAndTextAsync(userRegisterDto.Email);

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

                await authRepository.AddOtpAsync(otp);
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

            var tempUser = new TempUser();
            var hashedPassword = new PasswordHasher<TempUser>().HashPassword(tempUser, userRegisterDto.Password);

            tempUser.Id = Guid.NewGuid().ToString();
            tempUser.FirstName = userRegisterDto.FirstName;
            tempUser.LastName = userRegisterDto.LastName;
            tempUser.Username = userRegisterDto.Username;
            tempUser.Email = userRegisterDto.Email;
            tempUser.Phone = userRegisterDto.Phone;
            tempUser.Password = hashedPassword;
            tempUser.Avatar = "";
            tempUser.IsActive = true;
            tempUser.CreatedAt = DateTime.UtcNow;

            await authRepository.AddTempUserAsync(tempUser);

            return new APIResponse()
            {
                Success = true,
                Message = "Update OTP successfully!"
            };
        }

        private string GenerateOtp()
        {
            Random randomNumber = new Random();
            string randomNumberGenerator = randomNumber.Next(0, 1000000).ToString("D6");
            return randomNumberGenerator;
        }

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

        public async Task<APIResponse?> ResendOtp(string userEmail, string userFirstName)
        {
            if (await authRepository.FindAnyTempUserByEmailAsync(userEmail))
            {
                // update OTP
                var otp = await authRepository.GetOtpByEmailAndTextAsync(userEmail);

                if (otp is not null)
                {
                    otp.Text = GenerateOtp();
                    await authRepository.UpdateOtpAsync(otp);

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

        public async Task<bool> CheckUserActiveAsync(string email)
        {
            var user = await authRepository.GetUserByEmailAsync(email);

            if (user is null)
            {
                return false;
            }

            return user.IsActive;
        }

        public async Task<APIResponse?> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await authRepository.GetUserByEmailAsync(userLoginDto.Email);

            if (user is null)
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            var verifiedHashedPassword = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, userLoginDto.Password);
            if (verifiedHashedPassword == PasswordVerificationResult.Failed)
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            return new APIResponse()
            {
                Success = true,
                Data = new UserLoginResponseDto()
                {
                    Email = user.Email,
                    AccessToken = await CreateTokenResponse(user)
                }
            };
        }

        public async Task<string> CreateTokenResponse(User user)
        {
            string accessToken = CreateToken(user);
            string refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

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
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            await authRepository.SaveRefreshTokenAsync(user, refreshToken, 7);
            return refreshToken;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        //
        public async Task<APIResponse?> RefreshTokenAsync(UserRefreshTokenDto userRefreshTokenDto)
        {
            var apiRes = await ValidateRefreshTokenAsync(userRefreshTokenDto.UserId!);

            if (apiRes!.Success == false)
            {
                return apiRes;
            }

            User? user = apiRes.Data as User;

            return new APIResponse()
            {
                Success = true,
                Data = new UserRefreshTokenResponseDto()
                {
                    AccessToken = await CreateTokenResponse(user!)
                }
            };
        }

        private async Task<APIResponse?> ValidateRefreshTokenAsync(string userId)
        {
            if (!httpContextAccessor.HttpContext!.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "Cannot get refresh token."
                };
            }

            var user = await authRepository.GetUserByIdAsync(userId);

            if (user is null)
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "User is not found."
                };
            }

            if (user.RefreshToken != refreshToken)
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "Wrong refresh token."
                };
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "Refresh token is expired."
                };
            }

            return new APIResponse()
            {
                Success = true,
                Data = user
            };
        }

        //
        public async Task<APIResponse?> GetByEmailAsync(string email)
        {
            var user = await authRepository.GetUserByEmailAsync(email);

            if (user is null)
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "User does not exist."
                };
            }

            UserInfoDto userInfoDto = new UserInfoDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                Avatar = user.Avatar,
                IsActive = user.IsActive
            };

            return new APIResponse()
            {
                Success = true,
                Data = userInfoDto
            };
        }

        public async Task<APIResponse?> UpdateAsync(UserInfoDto userInfoDto)
        {
            var user = await authRepository.GetUserByIdAsync(userInfoDto.Id);

            if (user is null)
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "User does not exist."
                };
            }

            await authRepository.UpdateAsync(user, userInfoDto);

            return new APIResponse()
            {
                Success = true,
                Data = new UserInfoDto()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Email = user.Email,
                    Phone = user.Phone,
                    Avatar = user.Avatar,
                    IsActive = user.IsActive
                }
            };
        }
    }
}