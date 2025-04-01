using System.Text.Json.Serialization;

namespace todo_app_backend.Models
{
    public class Tag
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        
        [JsonIgnore]
        public virtual ICollection<TodoTaskTag> TodoTaskTags { get; set; } = [];

        [JsonIgnore]
        public virtual ICollection<UserTag> UserTags { get; set; } = new List<UserTag>();
    }
}