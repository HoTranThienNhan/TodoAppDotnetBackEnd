using todo_app_backend.Data;
using todo_app_backend.Models;
using Microsoft.EntityFrameworkCore;
using todo_app_backend.DTOs.Auth;
using todo_app_backend.Repositories.Contracts;

namespace todo_app_backend.Repositories
{
    public class AuthRepository(AppDbContext appDbContext) : IAuthRepository
    {
        public async Task<bool> FindAnyByIdAsync(string userId)
        {
            return await appDbContext.User.AnyAsync(user => user.Id == userId);
        }

        public async Task<bool> FindAnyByEmailAsync(string email)
        {
            return await appDbContext.User.AnyAsync(user => user.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(string id) {
            return await appDbContext.User.FindAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email) {
            return await appDbContext.User.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<TempUser?> GetTempUserByEmail(string userEmail)
        {
            return await appDbContext.TempUser.FirstOrDefaultAsync(user => user.Email == userEmail);
        }

        public async Task AddUserAsync(User user)
        {
            await appDbContext.User.AddAsync(user);
            await appDbContext.SaveChangesAsync();
        }

        public async Task AddTempUserAsync(TempUser tempUser)
        {
            await appDbContext.TempUser.AddAsync(tempUser);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<bool> FindAnyTempUserByEmailAsync(string email)
        {
            return await appDbContext.TempUser.AnyAsync(user => user.Email == email);
        }

        public async Task DeleteTempUser(TempUser tempUser)
        {
            appDbContext.TempUser.Remove(tempUser);
            await appDbContext.SaveChangesAsync();
        }

        public async Task AddOtpAsync(Otp otp)
        {
            await appDbContext.Otp.AddAsync(otp);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<Otp?> GetOtpByEmailAndTextAsync(string? userEmail, string? otpText)
        {
            if (userEmail is not null && otpText is not null)
            {
                return await appDbContext.Otp.FirstOrDefaultAsync(otp => otp.UserEmail == userEmail && otp.Text == otpText);
            }
            else if (userEmail is not null && otpText is null)
            {
                return await appDbContext.Otp.FirstOrDefaultAsync(otp => otp.UserEmail == userEmail);
            }
            else if (userEmail is null && otpText is not null)
            {
                return await appDbContext.Otp.FirstOrDefaultAsync(otp => otp.Text == otpText);
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateOtpAsync(Otp otp)
        {
            otp.Id = otp.Id;
            otp.Text = otp.Text;
            otp.ExpiryTime = DateTime.Now.AddMinutes(5);
            otp.CreatedAt = DateTime.Now;
            otp.UserEmail = otp.UserEmail;

            await appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user, UserInfoDto userInfoDto)
        {
            user.FirstName = userInfoDto.FirstName ?? user.FirstName;
            user.LastName = userInfoDto.LastName ?? user.LastName;
            user.Username = userInfoDto.Username ?? user.Username;
            user.Email = userInfoDto.Email ?? user.Email;
            user.Phone = userInfoDto.Phone ?? user.Phone;
            user.IsActive = userInfoDto.IsActive ?? user.IsActive;

            await appDbContext.SaveChangesAsync();
        }

        public async Task SaveRefreshTokenAsync(User user, string refreshToken, int expiryDays = 7) {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(expiryDays);
            await appDbContext.SaveChangesAsync();
        }
    }
}