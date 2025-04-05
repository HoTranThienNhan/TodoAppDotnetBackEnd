using System.Text.Json.Serialization;

namespace todo_app_backend.Models
{
    public class TodoSubtask
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        public string? TodoTaskId { get; set; } = string.Empty;

        [JsonIgnore]
        public TodoTask TodoTask { get; set; } = null!;
    }
}