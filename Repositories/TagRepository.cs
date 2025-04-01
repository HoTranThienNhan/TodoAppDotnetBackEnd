using Microsoft.EntityFrameworkCore;
using todo_app_backend.Data;
using todo_app_backend.DTOs.Tag;
using todo_app_backend.Models;
using todo_app_backend.Repositories.Contracts;

namespace todo_app_backend.Repositories
{
    public class TagRepository(AppDbContext appDbContext) : ITagRepository
    {
        public async Task<bool> FindAnyByNameAsync(string tagName) {
            return await appDbContext.Tag.AnyAsync(tag => tag.Name == tagName);
        }

        public async Task<bool> FindAnyUserTagAsync(string userId, string tagId) {
            return await appDbContext.UserTag.AnyAsync(userTag => userTag.UserId == userId && userTag.TagId == tagId);
        }

        public async Task AddAsync(Tag tag) {
            await appDbContext.Tag.AddAsync(tag);

            await appDbContext.SaveChangesAsync();
        }

        public async Task AddUserTagAsync(UserTag userTag) {
            await appDbContext.UserTag.AddAsync(userTag);

            await appDbContext.SaveChangesAsync();
        }

        public async Task<UserAllTagsResponseDto?> GetAllByUserIdAsync(string userId) {
            return await appDbContext.User
                .Include(t => t.UserTags)
                .ThenInclude(t => t.Tag)
                .Where(userTag => userTag.Id == userId)
                .Select(user => new UserAllTagsResponseDto() {
                    Tags = user.UserTags.Select(userTag => userTag.Tag).ToList(),
                }).FirstOrDefaultAsync();
        }

        public async Task<Tag?> GetByNameAsync(string tagName) {
            return await appDbContext.Tag.FirstOrDefaultAsync(tag => tag.Name == tagName);
        }
    }
}