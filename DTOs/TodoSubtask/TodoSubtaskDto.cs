namespace todo_app_backend.DTOs.TodoSubtask
{
    public class TodoSubtaskDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool? IsDone { get; set; }
    }
}