using System.Text.Json.Serialization;

namespace todo_app_backend.Models
{
    public class TodoTask
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsImportant { get; set; }
        public bool IsDone { get; set; } 
        public string UserId { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public virtual ICollection<TodoTaskTag> TodoTaskTags { get; set; } = [];
        public virtual ICollection<TodoSubtask> TodoSubtasks { get; set; } = [];
    }
}