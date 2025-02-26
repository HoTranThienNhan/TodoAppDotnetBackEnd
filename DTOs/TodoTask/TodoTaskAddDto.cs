using System.ComponentModel.DataAnnotations;
using todo_app_backend.DTOs.Tag;

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
        public bool? IsDeleted { get; set; } 

        [Required(ErrorMessage = "{0} is required.")]
        public string UserId { get; set; } = string.Empty;
        public ICollection<TagDto> Tags { get; set; } = [];
    }
}