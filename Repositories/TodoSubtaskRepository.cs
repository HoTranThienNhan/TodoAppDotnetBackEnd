using Microsoft.EntityFrameworkCore;
using todo_app_backend.Data;
using todo_app_backend.DTOs.TodoSubtask;
using todo_app_backend.Models;
using todo_app_backend.Repositories.Contracts;

namespace todo_app_backend.Repositories
{
    public class TodoSubtaskRepository(AppDbContext appDbContext) : ITodoSubtaskRepository
    {
        public async Task AddAsync(TodoSubtask todoSubtask)
        {
            await appDbContext.TodoSubtask.AddAsync(todoSubtask);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<TodoSubtask?> GetByIdAsync(string id) {
            return await appDbContext.TodoSubtask.FirstOrDefaultAsync(todoSubtask => todoSubtask.Id == id);
        }

        public async Task<bool> FindAnyByNameAsync(string name) {
            return await appDbContext.TodoSubtask.AnyAsync(todoSubtask => todoSubtask.Name == name);
        }

        public async Task UpdateAsync(TodoSubtask todoSubtask, TodoSubtaskDto todoSubtaskUpdateDto)
        {
            todoSubtask.Name = todoSubtaskUpdateDto.Name;
            todoSubtask.IsDone = todoSubtaskUpdateDto.IsDone ?? todoSubtask.IsDone;

            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TodoSubtask todoSubtask)
        {
            appDbContext.Remove(todoSubtask);
            await appDbContext.SaveChangesAsync();
        }
    }
}