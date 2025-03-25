using System.ComponentModel.DataAnnotations;

namespace todo_app_backend.DTOs.Auth
{
    public class UserRegisterConfirmationDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required.")]
        public string OtpText { get; set; } = string.Empty;
    }
}