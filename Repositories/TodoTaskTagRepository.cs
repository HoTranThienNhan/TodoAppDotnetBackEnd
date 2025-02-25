using Microsoft.EntityFrameworkCore;
using todo_app_backend.Contracts;
using todo_app_backend.Data;
using todo_app_backend.DTOs.TodoTaskTag;
using todo_app_backend.Models;

namespace todo_app_backend.Repositories
{
    public class TodoTaskTagRepository(AppDbContext appDbContext) : ITodoTaskTagRepository
    {
        public async Task<bool> FindAnyAsync(TodoTaskTagAddDto todoTaskTagAddDto) {
            return await appDbContext.TodoTaskTag.AnyAsync(todoTaskTag => 
                todoTaskTag.TodoTaskId == todoTaskTagAddDto.TodoTaskId 
                && todoTaskTag.TagId == todoTaskTagAddDto.TagId);
        }

        public async Task<TodoTaskTag> AddAsync(TodoTaskTagAddDto todoTaskTagAddDto) {
            var todoTaskTag = new TodoTaskTag() {
                TodoTaskId = todoTaskTagAddDto.TodoTaskId,
                TagId = todoTaskTagAddDto.TagId
            };

            await appDbContext.TodoTaskTag.AddAsync(todoTaskTag);
            await appDbContext.SaveChangesAsync();

            return todoTaskTag;
        }
    }
}