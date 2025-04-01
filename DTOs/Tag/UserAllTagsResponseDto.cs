namespace todo_app_backend.DTOs.Tag
{
    public class UserAllTagsResponseDto
    {
        public ICollection<Models.Tag> Tags { get; set; } = null!;
    }
}