using System.ComponentModel.DataAnnotations;

namespace todo_app_backend.DTOs.TodoSubtask
{
    public class TodoSubtaskAddDto
    {
        public List<TodoSubtaskDto> TodoSubtasks { get; set; } = null!;

        [Required(ErrorMessage = "{0} is required.")]
        public string TodoTaskId { get; set; } = string.Empty;
    }
}