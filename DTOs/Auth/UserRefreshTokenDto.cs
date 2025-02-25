namespace todo_app_backend.DTOs.Auth
{
    public class UserRefreshTokenDto
    {
        public string? UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}