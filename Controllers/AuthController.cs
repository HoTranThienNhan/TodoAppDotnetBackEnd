using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.Contracts;
using todo_app_backend.DTOs.Auth;

namespace todo_app_backend.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]

    public class AuthController(IAuthRepository authRepository) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto userRegisterDto) {
            var user = await authRepository.RegisterAsync(userRegisterDto);

            if (user is null) {
                return BadRequest("User is already existed!");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto userLoginDto) {
            if (!await authRepository.CheckUserActiveAsync(userLoginDto.Email)) {
                return BadRequest("User is inactive.");
            }

            var tokens = await authRepository.LoginAsync(userLoginDto);

            if (tokens is null) {
                return BadRequest("Invalid email or password.");
            }

            UserLoginResponseDto result = new UserLoginResponseDto() {
                Email = userLoginDto.Email,
                Tokens = tokens
            };

            return Ok(result);
        }

        [HttpPost("refreshToken")]
        [Authorize]
        public async Task<ActionResult> RefreshToken(UserRefreshTokenDto userRefreshTokenDt) {
            var tokens = await authRepository.RefreshTokenAsync(userRefreshTokenDt);

            if (tokens is null) {
                return BadRequest("Invalid User ID or Refresh Token");
            }

            return Ok(tokens);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult> GetProfileByEmail([FromQuery] string email) {
            var user = await authRepository.GetByEmailAsync(email);

            if (user is null) {
                return BadRequest("User does not exist.");
            }

            return Ok(user);
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<ActionResult> Update([FromBody] UserInfoDto userInfoDto) {
            var user = await authRepository.UpdateAsync(userInfoDto);

            if (user is null) {
                return BadRequest("User does not exist.");
            }

            return Ok(user);
        }
    }
}