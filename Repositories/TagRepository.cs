using Microsoft.EntityFrameworkCore;
using todo_app_backend.Data;
using todo_app_backend.DTOs.Tag;
using todo_app_backend.Models;

namespace todo_app_backend.Repositories
{
    public class TagRepository(AppDbContext appDbContext) : ITagRepository
    {
        public async Task<bool> FindAnyByNameAsync(TagAddDto tagAddDto) {
            return await appDbContext.Tag.AnyAsync(tag => tag.Name == tagAddDto.Name);
        }

        public async Task AddAsync(Tag tag) {
            await appDbContext.Tag.AddAsync(tag);
            await appDbContext.SaveChangesAsync();
        }
    }
}