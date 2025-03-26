namespace todo_app_backend.DTOs.Auth
{
    public class UserConfirmEmailRegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string OtpText { get; set; } = string.Empty;
        
    }
}