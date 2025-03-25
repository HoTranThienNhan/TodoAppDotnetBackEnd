using todo_app_backend.Models;
using todo_app_backend.DTOs.Auth;
using todo_app_backend.Helpers;

namespace todo_app_backend.Contracts
{
    public interface IAuthRepository
    {
        Task<bool> FindAnyByIdAsync(string userId) {
            throw new NotImplementedException();
        }

        Task<bool> CheckUserActiveAsync(string userId) {
            throw new NotImplementedException();
        }

        Task<APIResponse?> RegisterAsync(UserRegisterDto userRegisterDto) {
            throw new NotImplementedException();
        }

        Task<APIResponse?> AddOrUpdateOtp(string userEmail, string firstName) {
            throw new NotImplementedException();
        }

        Task<string> ValidateOtp(string userEmail, string otpText) {
            throw new NotImplementedException();
        }

        Task<User?> ConfirmRegisterAsync(UserRegisterConfirmationDto userRegisterConfirmationDto) {
            throw new NotImplementedException();
        }

        Task<UserTokensDto?> LoginAsync(UserLoginDto userLoginDto) {
            throw new NotImplementedException();
        }

        Task<UserTokensDto?> RefreshTokenAsync(UserRefreshTokenDto userRefreshTokenDto) {
            throw new NotImplementedException();
        }

        Task<UserInfoDto?> GetByEmailAsync(string email) {
            throw new NotImplementedException();
        }

        Task<UserInfoDto?> UpdateAsync(UserInfoDto userInfoDto) {
            throw new NotImplementedException();
        }
    }
}