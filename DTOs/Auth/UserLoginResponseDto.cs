namespace todo_app_backend.DTOs.Auth
{
    public class UserLoginResponseDto
    {
        public string Email { get; set; } = string.Empty;
        public UserTokensDto? Tokens { get; set; }
    }
}