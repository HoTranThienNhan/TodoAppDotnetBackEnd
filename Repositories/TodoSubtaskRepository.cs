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

        public async Task UpdateAsync(TodoSubtask todoSubtask, TodoSubtaskUpdateDto todoSubtaskUpdateDto)
        {
            todoSubtask.Id = todoSubtaskUpdateDto.Id;
            todoSubtask.Name = todoSubtaskUpdateDto.Name ?? todoSubtask.Name;
            todoSubtask.IsDone = todoSubtaskUpdateDto.IsDone ?? todoSubtask.IsDone;
            todoSubtask.TodoTaskId = todoSubtaskUpdateDto.TodoTaskId ?? todoSubtask.TodoTaskId;

            await appDbContext.SaveChangesAsync();
        }
    }
}