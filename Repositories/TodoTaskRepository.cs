using Microsoft.EntityFrameworkCore;
using todo_app_backend.Contracts;
using todo_app_backend.Data;
using todo_app_backend.DTOs.TodoSubtask;
using todo_app_backend.DTOs.TodoTask;
using todo_app_backend.Models;

namespace todo_app_backend.Repositories
{
    public class TodoTaskRepository(AppDbContext appDbContext) : ITodoTaskRepository
    {
        public async Task<bool> FindAnyAsync(string id) {
            return await appDbContext.TodoTask.AnyAsync(todoTask => todoTask.Id == id);
        }

        public async Task<TodoTask?> AddAsync(TodoTaskAddDto todoTaskAddDto) {
            var todoTask = new TodoTask() {
                Id = Guid.NewGuid().ToString(),
                Name = todoTaskAddDto.Name,
                Description = todoTaskAddDto.Description,
                Date = todoTaskAddDto.Date ?? DateTime.UtcNow,
                IsImportant = todoTaskAddDto.IsImportant ?? false ,
                IsDone = todoTaskAddDto.IsDone ?? false,
                UserId = todoTaskAddDto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await appDbContext.TodoTask.AddAsync(todoTask);
            await appDbContext.SaveChangesAsync();

            return todoTask;
        }

        public async Task<List<TodoTaskResponseDto>> GetAllByUserIdAsync(string userId) {
            var todoTask = await appDbContext.TodoTask
                .Include(t => t.TodoTaskTags)
                .ThenInclude(t => t.Tag)
                .Include(t => t.TodoSubtasks)
                .Where(todoTask => todoTask.UserId == userId)
                .Select(todoTask => new TodoTaskResponseDto() {
                    Id = todoTask.Id,
                    Name = todoTask.Name,
                    Description = todoTask.Description,
                    Date = todoTask.Date,
                    IsImportant = todoTask.IsImportant,
                    IsDone = todoTask.IsDone,
                    UserId = todoTask.UserId,
                    CreatedAt = todoTask.CreatedAt,
                    Tags = todoTask.TodoTaskTags.Select(todoTaskTag => todoTaskTag.Tag.Name).ToList(),
                    TodoSubTasks = todoTask.TodoSubtasks.Select(todoSubtask => new TodoSubtaskDto() {
                        Name = todoSubtask.Name,
                        IsDone = todoSubtask.IsDone
                    }).ToList()
                })
                .ToListAsync();

            return todoTask;
        }

        public async Task<TodoTaskResponseDto?> GetDetailsAsync(string id) {
            var todoTask = await appDbContext.TodoTask
                .Include(t => t.TodoTaskTags)
                .ThenInclude(t => t.Tag)
                .Include(t => t.TodoSubtasks)
                .Select(todoTask => new TodoTaskResponseDto() {
                    Id = todoTask.Id,
                    Name = todoTask.Name,
                    Description = todoTask.Description,
                    Date = todoTask.Date,
                    IsImportant = todoTask.IsImportant,
                    IsDone = todoTask.IsDone,
                    UserId = todoTask.UserId,
                    CreatedAt = todoTask.CreatedAt,
                    Tags = todoTask.TodoTaskTags.Select(todoTaskTag => todoTaskTag.Tag.Name).ToList(),
                    TodoSubTasks = todoTask.TodoSubtasks.Select(todoSubtask => new TodoSubtaskDto() {
                        Name = todoSubtask.Name,
                        IsDone = todoSubtask.IsDone
                    }).ToList()
                })
                .FirstOrDefaultAsync(todoTask => todoTask.Id == id);

            return todoTask;
        }

        public async Task<TodoTask?> UpdateAsync(TodoTaskUpdateDto todoTaskUpdateDto) {
            var todoTask = await appDbContext.TodoTask.FirstOrDefaultAsync(todoTask => todoTask.Id == todoTaskUpdateDto.Id);

            if (todoTask is null) {
                return null;
            }

            todoTask.Id = todoTask.Id;
            todoTask.Name = todoTaskUpdateDto.Name ?? todoTask.Name;
            todoTask.Description = todoTaskUpdateDto.Description ?? todoTask.Description;
            todoTask.Date = todoTaskUpdateDto.Date ?? todoTask.Date;
            todoTask.IsImportant = todoTaskUpdateDto.IsImportant ?? todoTask.IsImportant;
            todoTask.IsDone = todoTaskUpdateDto.IsDone ?? todoTask.IsDone;
            todoTask.UserId = todoTaskUpdateDto.UserId ?? todoTask.UserId;
            todoTask.CreatedAt = todoTask.CreatedAt;

            await appDbContext.SaveChangesAsync();

            return todoTask;
        }

        public async Task DeleteAsync(string id) {
            var todoTask = await appDbContext.TodoTask.FirstOrDefaultAsync(todoTask => todoTask.Id == id);

            if (todoTask is not null) {
                appDbContext.TodoTask.Remove(todoTask);
                await appDbContext.SaveChangesAsync();
            }
        }
    }
}