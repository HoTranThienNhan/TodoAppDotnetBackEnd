using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.DTOs.Auth;
using todo_app_backend.Repositories;

namespace todo_app_backend.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [EnableCors("AllowSpecificOrigins")] 

    public class AuthController(IAuthRepository authRepository) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto userRegisterDto) {
            var response = await authRepository.AddOrUpdateOtp(userRegisterDto);

            return Ok(response);
        }

        [HttpPost("confirmEmailRegister")]
        public async Task<ActionResult> ConfirmRegister(UserConfirmEmailRegisterDto userConfirmEmailRegisterDto) {
            var response = await authRepository.RegisterAsync(userConfirmEmailRegisterDto.Email, userConfirmEmailRegisterDto.OtpText);

            if (!response!.Success) {
                return BadRequest(response);
            } else {
                return Ok(response);
            }
        }

        [HttpPost("resendCode")]
        public async Task<ActionResult> ResendOtp(UserResendOTPDto userResendOTPDto) {
            var response = await authRepository.ResendOtp(userResendOTPDto.Email, userResendOTPDto.FirstName);

            if (!response!.Success) {
                return BadRequest(response);
            } else {
                return Ok(response);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto userLoginDto) {
            if (!await authRepository.CheckUserActiveAsync(userLoginDto.Email)) {
                return BadRequest("User is inactive.");
            }

            var accessToken = await authRepository.LoginAsync(userLoginDto);

            if (accessToken is null) {
                return BadRequest("Invalid email or password.");
            }

            UserLoginResponseDto result = new UserLoginResponseDto() {
                Email = userLoginDto.Email,
                AccessToken = accessToken
            };

            return Ok(result);
        }

        [HttpPost("refreshToken")]
        [Authorize]
        public async Task<ActionResult> RefreshToken(UserRefreshTokenDto userRefreshTokenDt) {
            var accessToken = await authRepository.RefreshTokenAsync(userRefreshTokenDt);

            if (accessToken is null) {
                return BadRequest("Invalid User ID or Refresh Token");
            }

            UserRefreshTokenResponseDto result = new UserRefreshTokenResponseDto() {
                AccessToken = accessToken
            };

            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile")]
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