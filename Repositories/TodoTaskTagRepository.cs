using Microsoft.EntityFrameworkCore;
using todo_app_backend.Data;
using todo_app_backend.Models;
using todo_app_backend.Repositories.Contracts;

namespace todo_app_backend.Repositories
{
    public class TodoTaskTagRepository(AppDbContext appDbContext) : ITodoTaskTagRepository
    {
        public async Task<List<TodoTaskTag>> GetByTodoTaskId(string todoTaskId) {
            return await appDbContext.TodoTaskTag
                .Where(todoTaskTag => todoTaskTag.TodoTaskId == todoTaskId)
                .ToListAsync();
        }
        public async Task AddAsync(TodoTaskTag todoTaskTag) {
            await appDbContext.TodoTaskTag.AddAsync(todoTaskTag);
            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(List<TodoTaskTag> todoTaskTags) {
            appDbContext.TodoTaskTag.RemoveRange(todoTaskTags);

            await appDbContext.SaveChangesAsync();
        }
    }
}