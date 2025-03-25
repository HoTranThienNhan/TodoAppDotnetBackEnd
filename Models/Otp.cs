namespace todo_app_backend.Models
{
    public class Otp
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        
    }
}