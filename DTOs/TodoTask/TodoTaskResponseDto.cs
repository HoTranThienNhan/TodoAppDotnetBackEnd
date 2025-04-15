using todo_app_backend.DTOs.TodoSubtask;

namespace todo_app_backend.DTOs.TodoTask
{
    public class TodoTaskResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsImportant { get; set; }
        public bool IsDone { get; set; } 
        public bool IsDeleted { get; set; } 
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Models.Tag> Tags { get; set; } = [];
        public virtual ICollection<TodoSubtaskDto> TodoSubtasks { get; set; } = [];
    }
}