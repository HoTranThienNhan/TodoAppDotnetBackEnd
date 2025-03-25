using System.ComponentModel.DataAnnotations;

namespace todo_app_backend.DTOs.Auth
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; } = string.Empty;
        
        [StringLength(maximumLength: 10, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "{0} is required.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(maximumLength: 11, ErrorMessage = "{0} number must be {2}-{1} characters.", MinimumLength = 10)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required.")]
        public string Password { get; set; } = string.Empty;

        public string? OtpText { get; set; } = string.Empty;
    }
}