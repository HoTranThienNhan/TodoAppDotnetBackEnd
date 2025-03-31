using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.DTOs.Auth;
using todo_app_backend.Services.Contracts;

namespace todo_app_backend.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [EnableCors("AllowSpecificOrigins")] 

    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto userRegisterDto) {
            var response = await authService.AddOrUpdateOtp(userRegisterDto);

            return Ok(response);
        }

        [HttpPost("confirmEmailRegister")]
        public async Task<ActionResult> ConfirmRegister(UserConfirmEmailRegisterDto userConfirmEmailRegisterDto) {
            var response = await authService.RegisterAsync(userConfirmEmailRegisterDto.Email, userConfirmEmailRegisterDto.OtpText);

            if (!response!.Success) {
                return BadRequest(response);
            } else {
                return Ok(response);
            }
        }

        [HttpPost("resendCode")]
        public async Task<ActionResult> ResendOtp(UserResendOTPDto userResendOTPDto) {
            var response = await authService.ResendOtp(userResendOTPDto.Email, userResendOTPDto.FirstName);

            if (!response!.Success) {
                return BadRequest(response);
            } else {
                return Ok(response);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto userLoginDto) {
            if (!await authService.CheckUserActiveAsync(userLoginDto.Email)) {
                return BadRequest("User is inactive.");
            }

            var accessToken = await authService.LoginAsync(userLoginDto);

            if (accessToken!.Success == false) {
                return BadRequest(accessToken);
            }

            return Ok(accessToken);
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult> RefreshToken(UserRefreshTokenDto userRefreshTokenDt) {
            var accessToken = await authService.RefreshTokenAsync(userRefreshTokenDt);

            if (accessToken!.Success == false) {
                return BadRequest(accessToken);
            }

            return Ok(accessToken);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult> GetProfileByEmail([FromQuery] string email) {
            var user = await authService.GetByEmailAsync(email);

            if (user!.Success == false) {
                return BadRequest(user);
            }

            return Ok(user);
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<ActionResult> Update([FromBody] UserInfoDto userInfoDto) {
            var user = await authService.UpdateAsync(userInfoDto);

            if (user!.Success == false) {
                return BadRequest(user);
            }

            return Ok(user);
        }
    }
}