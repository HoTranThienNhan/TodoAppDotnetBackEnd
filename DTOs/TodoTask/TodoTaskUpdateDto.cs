using System.ComponentModel.DataAnnotations;
using todo_app_backend.DTOs.TodoSubtask;

namespace todo_app_backend.DTOs.TodoTask
{
    public class TodoTaskUpdateDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsImportant { get; set; }
        public bool? IsDone { get; set; } 
        public bool? IsDeleted { get; set; } 
        public string? UserId { get; set; }
        public virtual ICollection<Models.Tag> Tags { get; set; } = [];
        public virtual ICollection<TodoSubtaskDto> TodoSubtasks { get; set; } = [];

    }
}