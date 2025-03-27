using todo_app_backend.Models;
using todo_app_backend.DTOs.Auth;

namespace todo_app_backend.Repositories.Contracts
{
    public interface IAuthRepository
    {
        Task<bool> FindAnyByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        Task<bool> FindAnyByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        Task AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        Task<User?> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        Task<User?> GetUserByIdAsync(string id) {
            throw new NotImplementedException();
        }

        Task<Otp?> GetOtpByEmailAndTextAsync(string? userEmail = null, string? otpText = null)
        {
            throw new NotImplementedException();
        }

        Task<bool> FindAnyTempUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        Task AddTempUserAsync(TempUser tempUser)
        {
            throw new NotImplementedException();
        }

        Task DeleteTempUser(TempUser tempUser)
        {
            throw new NotImplementedException();
        }

        Task AddOtpAsync(Otp otp)
        {
            throw new NotImplementedException();
        }

        Task<TempUser?> GetTempUserByEmail(string userEmail)
        {
            throw new NotImplementedException();
        }

        Task UpdateOtpAsync(Otp otp)
        {
            throw new NotImplementedException();
        }

        Task UpdateAsync(User user, UserInfoDto userInfoDto)
        {
            throw new NotImplementedException();
        }

        Task SaveRefreshTokenAsync(User user, string refreshToken, int expiryDays = 7) {
            throw new NotImplementedException();
        }
    }
}