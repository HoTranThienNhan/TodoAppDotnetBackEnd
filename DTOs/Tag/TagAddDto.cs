using System.ComponentModel.DataAnnotations;

namespace todo_app_backend.DTOs.Tag
{
    public class TagAddDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required.")]
        public string UserId { get; set; } = string.Empty;
    }
}