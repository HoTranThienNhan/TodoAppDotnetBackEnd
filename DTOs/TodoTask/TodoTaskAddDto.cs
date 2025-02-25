using System.ComponentModel.DataAnnotations;

namespace todo_app_backend.DTOs.TodoTask
{
    public class TodoTaskAddDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(maximumLength: 100, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "{0} is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required.")]
        public DateTime? Date { get; set; }
        public bool? IsImportant { get; set; }
        public bool? IsDone { get; set; } 

        [Required(ErrorMessage = "{0} is required.")]
        public string UserId { get; set; } = string.Empty;
    }
}