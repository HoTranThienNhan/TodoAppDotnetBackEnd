using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using todo_app_backend.Data;
using todo_app_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using todo_app_backend.Contracts;
using todo_app_backend.DTOs.Auth;

namespace todo_app_backend.Repositories
{
    public class AuthRepository(AppDbContext appDbContext, IConfiguration configuration) : IAuthRepository
    {
        public async Task<bool> FindAnyByIdAsync(string userId) {
            return await appDbContext.User.AnyAsync(user => user.Id == userId);
        }

        public async Task<bool> CheckUserActiveAsync(string email) {
            var user = await appDbContext.User.FirstOrDefaultAsync(user => user.Email == email);

            if (user is null) {
                return false;
            } 

            return user.IsActive;
        }

        public async Task<User?> RegisterAsync(UserRegisterDto userRegisterDto) {
            if (await appDbContext.User.AnyAsync(user => user.Email == userRegisterDto.Email)) {
                return null;
            }

            var user = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, userRegisterDto.Password);

            user.Id = Guid.NewGuid().ToString();
            user.Fullname = userRegisterDto.Fullname;
            user.Email = userRegisterDto.Email;
            user.Phone = userRegisterDto.Phone;
            user.Password = hashedPassword;
            user.IsActive = true;
            user.CreatedAt = DateTime.UtcNow;

            await appDbContext.User.AddAsync(user);
            await appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<UserTokensDto?> LoginAsync(UserLoginDto userLoginDto) {
            var user = await appDbContext.User.FirstOrDefaultAsync(user => user.Email == userLoginDto.Email);

            if (user is null) {
                return null;
            }

            var verifiedHashedPassword = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, userLoginDto.Password);
            if (verifiedHashedPassword == PasswordVerificationResult.Failed) {
                return null;
            }

            var tokens = await CreateTokenResponse(user);
            return tokens;
        }

        public async Task<UserTokensDto> CreateTokenResponse(User user) {
            return new UserTokensDto {
                AccessToken = CreateToken(user!),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user!)
            };
        }

        private string CreateToken(User user) {
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

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user) {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await appDbContext.SaveChangesAsync();
            return refreshToken;
        }

        private string GenerateRefreshToken() {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<User?> ValidateRefreshTokenAsync(string userId, string refreshToken) {
            var user = await appDbContext.User.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) {
                return null;
            }
            return user;
        }

        public async Task<UserTokensDto?> RefreshTokenAsync(UserRefreshTokenDto userRefreshTokenDto) {
            var user = await ValidateRefreshTokenAsync(userRefreshTokenDto.UserId!, userRefreshTokenDto.RefreshToken);

            if (user is null) {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        public async Task<UserInfoDto?> GetByEmailAsync(string email) {
            var user = await appDbContext.User.FirstOrDefaultAsync(user => user.Email == email);

            if (user is null) {
                return null;
            }

            return new UserInfoDto() {
                Id = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive
            };
        }

        public async Task<UserInfoDto?> UpdateAsync(UserInfoDto userInfoDto) {
            var user = await appDbContext.User.FirstOrDefaultAsync(user => user.Id == userInfoDto.Id);

            if (user is null) {
                return null;
            }

            user.Fullname = userInfoDto.Fullname ?? user.Fullname;
            user.Email = userInfoDto.Email ?? user.Email;
            user.Phone = userInfoDto.Phone ?? user.Phone;
            user.IsActive = userInfoDto.IsActive ?? user.IsActive;

            await appDbContext.SaveChangesAsync();

            return new UserInfoDto() {
                Id = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive
            };
        }
    }
}