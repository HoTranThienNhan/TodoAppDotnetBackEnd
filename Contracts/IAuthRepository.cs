using todo_app_backend.Models;
using todo_app_backend.DTOs.Auth;

namespace todo_app_backend.Contracts
{
    public interface IAuthRepository
    {
        Task<bool> FindAnyByIdAsync(string userId) {
            throw new NotImplementedException();
        }

        Task<User?> RegisterAsync(UserRegisterDto userRegisterDto) {
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
    }
}