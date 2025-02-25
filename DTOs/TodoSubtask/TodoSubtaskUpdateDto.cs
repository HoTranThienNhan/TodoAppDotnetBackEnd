namespace todo_app_backend.DTOs.TodoSubtask
{
    public class TodoSubtaskUpdateDto
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public bool? IsDone { get; set; }
        public string? TodoTaskId { get; set; } = string.Empty;
    }
}