namespace todo_app_backend.DTOs.Auth
{
    public class UserResendOTPDto
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
    }
}