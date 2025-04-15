using Microsoft.EntityFrameworkCore;
using todo_app_backend.Data;
using todo_app_backend.DTOs.TodoSubtask;
using todo_app_backend.DTOs.TodoTask;
using todo_app_backend.Enums.TodoTask;
using todo_app_backend.Models;
using todo_app_backend.Repositories.Contracts;

namespace todo_app_backend.Repositories
{
    public class TodoTaskRepository(AppDbContext appDbContext) : ITodoTaskRepository
    {
        public async Task<bool> FindAnyAsync(string id) {
            return await appDbContext.TodoTask.AnyAsync(todoTask => todoTask.Id == id);
        }

        public async Task<TodoTask?> GetByIdAsync(string id) {
            return await appDbContext.TodoTask
            .Include(t => t.TodoTaskTags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(todoTask => todoTask.Id == id);
        }

        public async Task AddAsync(TodoTask todoTask) {
            await appDbContext.TodoTask.AddAsync(todoTask);
            await appDbContext.SaveChangesAsync();
        }

        public async Task AddTodoTaskTagAsync(TodoTaskTag todoTaskTag) {
            await appDbContext.TodoTaskTag.AddAsync(todoTaskTag);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<List<TodoTaskResponseDto>> GetAllWithFilterAndSearchByUserIdAsync(string? userId, string? filter, string? search, bool? isDeleted) {
            if (search is null) {
                if (filter is null) {
                    return await appDbContext.TodoTask
                    .Include(t => t.TodoTaskTags)
                    .ThenInclude(t => t.Tag)
                    .Include(t => t.TodoSubtasks)
                    .Where(todoTask => todoTask.UserId == userId && todoTask.IsDeleted == isDeleted)
                    .Select(todoTask => new TodoTaskResponseDto() {
                        Id = todoTask.Id,
                        Name = todoTask.Name,
                        Description = todoTask.Description,
                        Date = todoTask.Date,
                        IsImportant = todoTask.IsImportant,
                        IsDone = todoTask.IsDone,
                        IsDeleted = todoTask.IsDeleted,
                        UserId = todoTask.UserId,
                        CreatedAt = todoTask.CreatedAt,
                        Tags = todoTask.TodoTaskTags.Select(todoTaskTag => todoTaskTag.Tag).ToList(),
                        TodoSubtasks = todoTask.TodoSubtasks.Select(todoSubtask => new TodoSubtaskDto() {
                            Id = todoSubtask.Id,
                            Name = todoSubtask.Name,
                            IsDone = todoSubtask.IsDone
                        }).ToList()
                    })
                    .ToListAsync();
                } else if (filter.Equals(GetAllFilter.Today.ToString())) {
                    return await appDbContext.TodoTask
                        .Include(t => t.TodoTaskTags)
                        .ThenInclude(t => t.Tag)
                        .Include(t => t.TodoSubtasks)
                        .Where(todoTask => todoTask.UserId == userId 
                            && todoTask.Date.Date.CompareTo(DateTime.UtcNow.Date) == 0
                            && todoTask.IsDeleted == isDeleted)
                        .Select(todoTask => new TodoTaskResponseDto() {
                            Id = todoTask.Id,
                            Name = todoTask.Name,
                            Description = todoTask.Description,
                            Date = todoTask.Date,
                            IsImportant = todoTask.IsImportant,
                            IsDone = todoTask.IsDone,
                            IsDeleted = todoTask.IsDeleted,
                            UserId = todoTask.UserId,
                            CreatedAt = todoTask.CreatedAt,
                            Tags = todoTask.TodoTaskTags.Select(todoTaskTag => todoTaskTag.Tag).ToList(),
                            TodoSubtasks = todoTask.TodoSubtasks.Select(todoSubtask => new TodoSubtaskDto() {
                                Id = todoSubtask.Id,
                                Name = todoSubtask.Name,
                                IsDone = todoSubtask.IsDone
                            }).ToList()
                        })
                        .ToListAsync();
                }
                else if (filter.Equals(GetAllFilter.Upcoming.ToString())) {
                    return await appDbContext.TodoTask
                        .Include(t => t.TodoTaskTags)
                        .ThenInclude(t => t.Tag)
                        .Include(t => t.TodoSubtasks)
                        .Where(todoTask => todoTask.UserId == userId 
                            && todoTask.Date.Date.CompareTo(DateTime.UtcNow.Date) > 0
                            && todoTask.IsDeleted == isDeleted)
                        .Select(todoTask => new TodoTaskResponseDto() {
                            Id = todoTask.Id,
                            Name = todoTask.Name,
                            Description = todoTask.Description,
                            Date = todoTask.Date,
                            IsImportant = todoTask.IsImportant,
                            IsDone = todoTask.IsDone,
                            IsDeleted = todoTask.IsDeleted,
                            UserId = todoTask.UserId,
                            CreatedAt = todoTask.CreatedAt,
                            Tags = todoTask.TodoTaskTags.Select(todoTaskTag => todoTaskTag.Tag).ToList(),
                            TodoSubtasks = todoTask.TodoSubtasks.Select(todoSubtask => new TodoSubtaskDto() {
                                Id = todoSubtask.Id,
                                Name = todoSubtask.Name,
                                IsDone = todoSubtask.IsDone
                            }).ToList()
                        })
                        .ToListAsync();
                } else if (filter.Equals(GetAllFilter.Done.ToString())) { 
                    return await appDbContext.TodoTask
                        .Include(t => t.TodoTaskTags)
                        .ThenInclude(t => t.Tag)
                        .Include(t => t.TodoSubtasks)
                        .Where(todoTask => todoTask.UserId == userId 
                            && todoTask.IsDone == true
                            && todoTask.IsDeleted == isDeleted)
                        .Select(todoTask => new TodoTaskResponseDto() {
                            Id = todoTask.Id,
                            Name = todoTask.Name,
                            Description = todoTask.Description,
                            Date = todoTask.Date,
                            IsImportant = todoTask.IsImportant,
                            IsDone = todoTask.IsDone,
                            IsDeleted = todoTask.IsDeleted,
                            UserId = todoTask.UserId,
                            CreatedAt = todoTask.CreatedAt,
                            Tags = todoTask.TodoTaskTags.Select(todoTaskTag => todoTaskTag.Tag).ToList(),
                            TodoSubtasks = todoTask.TodoSubtasks.Select(todoSubtask => new TodoSubtaskDto() {
                                Id = todoSubtask.Id,
                                Name = todoSubtask.Name,
                                IsDone = todoSubtask.IsDone
                            }).ToList()
                        })
                        .ToListAsync();
                } else if (filter.Equals(GetAllFilter.Important.ToString())) { 
                    return await appDbContext.TodoTask
                        .Include(t => t.TodoTaskTags)
                        .ThenInclude(t => t.Tag)
                        .Include(t => t.TodoSubtasks)
                        .Where(todoTask => todoTask.UserId == userId 
                            && todoTask.IsImportant == true
                            && todoTask.IsDeleted == isDeleted)
                        .Select(todoTask => new TodoTaskResponseDto() {
                            Id = todoTask.Id,
                            Name = todoTask.Name,
                            Description = todoTask.Description,
                            Date = todoTask.Date,
                            IsImportant = todoTask.IsImportant,
                            IsDone = todoTask.IsDone,
                            IsDeleted = todoTask.IsDeleted,
                            UserId = todoTask.UserId,
                            CreatedAt = todoTask.CreatedAt,
                            Tags = todoTask.TodoTaskTags.Select(todoTaskTag => todoTaskTag.Tag).ToList(),
                            TodoSubtasks = todoTask.TodoSubtasks.Select(todoSubtask => new TodoSubtaskDto() {
                                Id = todoSubtask.Id,
                                Name = todoSubtask.Name,
                                IsDone = todoSubtask.IsDone
                            }).ToList()
                        })
                        .ToListAsync();
                } else {
                    return [];
                }
            } else {    // search is not null
                return await appDbContext.TodoTask
                        .Include(t => t.TodoTaskTags)
                        .ThenInclude(t => t.Tag)
                        .Include(t => t.TodoSubtasks)
                        .Where(todoTask => todoTask.UserId == userId 
                            && todoTask.Name.Contains(search)
                            && todoTask.IsDeleted == isDeleted)
                        .Select(todoTask => new TodoTaskResponseDto() {
                            Id = todoTask.Id,
                            Name = todoTask.Name,
                            Description = todoTask.Description,
                            Date = todoTask.Date,
                            IsImportant = todoTask.IsImportant,
                            IsDone = todoTask.IsDone,
                            IsDeleted = todoTask.IsDeleted,
                            UserId = todoTask.UserId,
                            CreatedAt = todoTask.CreatedAt,
                            Tags = todoTask.TodoTaskTags.Select(todoTaskTag => todoTaskTag.Tag).ToList(),
                            TodoSubtasks = todoTask.TodoSubtasks.Select(todoSubtask => new TodoSubtaskDto() {
                                Id = todoSubtask.Id,
                                Name = todoSubtask.Name,
                                IsDone = todoSubtask.IsDone
                            }).ToList()
                        })
                        .ToListAsync();
            }
        }

        public async Task<TodoTaskResponseDto?> GetDetailsAsync(string id) {
            return await appDbContext.TodoTask.Include(t => t.TodoTaskTags)
            .ThenInclude(t => t.Tag)
            .Include(t => t.TodoSubtasks)
            .Select(todoTask => new TodoTaskResponseDto() {
                Id = todoTask.Id,
                Name = todoTask.Name,
                Description = todoTask.Description,
                Date = todoTask.Date,
                IsImportant = todoTask.IsImportant,
                IsDone = todoTask.IsDone,
                IsDeleted = todoTask.IsDeleted,
                UserId = todoTask.UserId,
                CreatedAt = todoTask.CreatedAt,
                Tags = todoTask.TodoTaskTags.Select(todoTaskTag => todoTaskTag.Tag).ToList(),
                TodoSubtasks = todoTask.TodoSubtasks.Select(todoSubtask => new TodoSubtaskDto() {
                    Id = todoSubtask.Id,
                    Name = todoSubtask.Name,
                    IsDone = todoSubtask.IsDone
                }).ToList()
            })
            .FirstOrDefaultAsync(todoTask => todoTask.Id == id);
        }

        public async Task<TodoTask?> UpdateAsync(TodoTask todoTask, TodoTaskUpdateDto todoTaskUpdateDto) {
            todoTask.Id = todoTask.Id;
            todoTask.Name = todoTaskUpdateDto.Name ?? todoTask.Name;
            todoTask.Description = todoTaskUpdateDto.Description ?? todoTask.Description;
            todoTask.Date = todoTaskUpdateDto.Date ?? todoTask.Date;
            todoTask.IsImportant = todoTaskUpdateDto.IsImportant ?? todoTask.IsImportant;
            todoTask.IsDone = todoTaskUpdateDto.IsDone ?? todoTask.IsDone;
            todoTask.IsDeleted = todoTaskUpdateDto.IsDeleted ?? todoTask.IsDeleted;
            todoTask.UserId = todoTaskUpdateDto.UserId ?? todoTask.UserId;
            todoTask.CreatedAt = todoTask.CreatedAt;

            await appDbContext.SaveChangesAsync();

            return todoTask;
        }

        public async Task DeleteAsync(TodoTask todoTask) {
            appDbContext.TodoTask.Remove(todoTask);
            await appDbContext.SaveChangesAsync();
        }
    }
}