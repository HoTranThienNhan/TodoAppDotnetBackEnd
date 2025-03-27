using Microsoft.EntityFrameworkCore;
using todo_app_backend.Data;
using todo_app_backend.DTOs.TodoSubtask;
using todo_app_backend.Models;
using todo_app_backend.Repositories.Contracts;

namespace todo_app_backend.Repositories
{
    public class TodoSubtaskRepository(AppDbContext appDbContext) : ITodoSubtaskRepository
    {
        public async Task<TodoSubtaskAddDto?> AddAsync(TodoSubtaskAddDto todoSubtaskAddDto) {
            foreach(var todoSubtaskAdd in todoSubtaskAddDto.TodoSubtasks) {
                var todoSubtask = new TodoSubtask() {
                    Id = Guid.NewGuid().ToString(),
                    Name = todoSubtaskAdd.Name,
                    IsDone = todoSubtaskAdd.IsDone ?? false,
                    TodoTaskId = todoSubtaskAddDto.TodoTaskId
                };

                await appDbContext.TodoSubtask.AddAsync(todoSubtask);
                await appDbContext.SaveChangesAsync();
            }

            return todoSubtaskAddDto;
        } 

        public async Task<TodoSubtask?> UpdateAsync(TodoSubtaskUpdateDto todoSubtaskUpdateDto) {
            var todoSubtask = await appDbContext.TodoSubtask.FirstOrDefaultAsync(todoSubtask => todoSubtask.Id == todoSubtaskUpdateDto.Id);

            if (todoSubtask is null) {
                return null;
            }

            todoSubtask.Id = todoSubtaskUpdateDto.Id;
            todoSubtask.Name = todoSubtaskUpdateDto.Name ?? todoSubtask.Name;
            todoSubtask.IsDone = todoSubtaskUpdateDto.IsDone ?? todoSubtask.IsDone;
            todoSubtask.TodoTaskId = todoSubtaskUpdateDto.TodoTaskId ?? todoSubtask.TodoTaskId;

            await appDbContext.SaveChangesAsync();

            return todoSubtask;
        }
    }
}