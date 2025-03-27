using todo_app_backend.DTOs.Auth;
using todo_app_backend.Helpers;
using todo_app_backend.Models;

namespace todo_app_backend.Services.Contracts
{
    public interface IAuthService
    {
        Task<APIResponse?> RegisterAsync(string userEmail, string otpText)
        {
            throw new NotImplementedException();
        }

        Task<string> ValidateOtp(string userEmail, string otpText)
        {
            throw new NotImplementedException();
        }

        Task<APIResponse?> AddOrUpdateOtp(UserRegisterDto userRegisterDto)
        {
            throw new NotImplementedException();
        }

        Task<APIResponse?> ResendOtp(string userEmail, string userFirstName)
        {
            throw new NotImplementedException();
        }

        Task<bool> CheckUserActiveAsync(string userId)
        {
            throw new NotImplementedException();
        }

        Task<APIResponse?> LoginAsync(UserLoginDto userLoginDto)
        {
            throw new NotImplementedException();
        }

        Task<APIResponse?> RefreshTokenAsync(UserRefreshTokenDto userRefreshTokenDto)
        {
            throw new NotImplementedException();
        }

        Task<string> GenerateAndSaveRefreshTokenAsync(User user) {
            throw new NotImplementedException();
        }

        Task<APIResponse?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        Task<APIResponse?> UpdateAsync(UserInfoDto userInfoDto)
        {
            throw new NotImplementedException();
        }
    }
}