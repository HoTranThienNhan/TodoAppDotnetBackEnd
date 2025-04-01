using System.Text.Json.Serialization;

namespace todo_app_backend.Models
{
    public class UserTag
    {
        public string UserId { get; set; } = string.Empty;

        [JsonIgnore]
        public User User { get; set; } = null!;

        public string TagId { get; set; } = string.Empty;

        [JsonIgnore]
        public Tag Tag { get; set; } = null!;
    }
}