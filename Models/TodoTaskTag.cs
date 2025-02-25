using System.Text.Json.Serialization;

namespace todo_app_backend.Models
{
    public class TodoTaskTag
    {
        public string TodoTaskId { get; set; } = string.Empty;

        [JsonIgnore]
        public TodoTask TodoTask { get; set; } = null!;
        
        public string TagId { get; set; } = string.Empty;

        [JsonIgnore]
        public Tag Tag { get; set; } = null!;
    }
}