using todo_app_backend.DTOs.TodoTask;
using todo_app_backend.Helpers;
using todo_app_backend.Models;
using todo_app_backend.Repositories.Contracts;
using todo_app_backend.Services.Contracts;

namespace todo_app_backend.Services
{
    public class TodoTaskService(
        ITodoTaskRepository todoTaskRepository,
        IAuthRepository authRepository,
        ITodoTaskTagRepository todoTaskTagRepository,
        ITodoSubtaskRepository todoSubtaskRepository) : ITodoTaskService
    {
        public async Task<bool> FindAnyAsync(string id)
        {
            if (!await todoTaskRepository.FindAnyAsync(id))
            {
                return false;
            }

            return true;
        }

        public async Task<APIResponse?> AddAsync(TodoTaskAddDto todoTaskAddDto)
        {
            if (!await authRepository.FindAnyByIdAsync(todoTaskAddDto.UserId))
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "UserID does not exist."
                };
            }

            var todoTask = new TodoTask()
            {
                Id = Guid.NewGuid().ToString(),
                Name = todoTaskAddDto.Name,
                Description = todoTaskAddDto.Description,
                Date = todoTaskAddDto.Date ?? DateTime.UtcNow,
                IsImportant = todoTaskAddDto.IsImportant ?? false,
                IsDone = todoTaskAddDto.IsDone ?? false,
                IsDeleted = todoTaskAddDto.IsDeleted ?? false,
                UserId = todoTaskAddDto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await todoTaskRepository.AddAsync(todoTask);

            foreach (var tag in todoTaskAddDto.Tags)
            {
                var todoTaskTag = new TodoTaskTag()
                {
                    TodoTaskId = todoTask.Id,
                    TagId = tag.Id
                };
                await todoTaskRepository.AddTodoTaskTagAsync(todoTaskTag);
            }

            return new APIResponse()
            {
                Success = true,
                Data = todoTask
            };
        }

        public async Task<APIResponse?> GetAllWithFilterAndSearchByUserIdAsync(string? userId, string? filter, string? search, bool? isDeleted)
        {
            if (search is not null && filter is not null)
            {
                return new APIResponse()
                {
                    Success = false,
                    Data = "Cannot find All Todo Tasks with both search and filter"
                };
            }

            return new APIResponse()
            {
                Success = true,
                Data = await todoTaskRepository.GetAllWithFilterAndSearchByUserIdAsync(userId, filter, search, isDeleted)
            };
        }

        public async Task<APIResponse?> GetDetailsAsync(string id)
        {
            return new APIResponse()
            {
                Success = true,
                Data = await todoTaskRepository.GetDetailsAsync(id)
            };
        }

        public async Task<APIResponse?> UpdateAsync(TodoTaskUpdateDto todoTaskUpdateDto)
        {
            var todoTask = await todoTaskRepository.GetByIdAsync(todoTaskUpdateDto.Id);

            if (todoTask is null)
            {
                return new APIResponse()
                {
                    Success = true,
                    Message = "Todo task does not exist."
                };
            }

            await todoTaskRepository.UpdateAsync(todoTask, todoTaskUpdateDto);

            // delete all the current Todo Task Tags then add the updated Todo Task Tags
            List<TodoTaskTag>? deletedTodoTaskTags = await todoTaskTagRepository.GetByTodoTaskId(todoTask.Id);
            await todoTaskTagRepository.DeleteAsync(deletedTodoTaskTags);

            foreach (var tag in todoTaskUpdateDto.Tags)
            {
                TodoTaskTag todoTaskTag = new TodoTaskTag()
                {
                    TagId = tag.Id,
                    TodoTaskId = todoTask.Id
                };
                await todoTaskTagRepository.AddAsync(todoTaskTag);
            }

            // update existed todoSubtasks, add new todoSubtasks, delete removed todoSubtasks
            foreach (var updatedTodoSubtask in todoTaskUpdateDto.TodoSubtasks)
            {
                // update
                if (updatedTodoSubtask.Id != "")
                {
                    TodoSubtask? todoSubtask = await todoSubtaskRepository.GetByIdAsync(updatedTodoSubtask.Id);

                    if (todoSubtask is not null)
                    {
                        todoSubtask.Name = todoSubtask.Name;
                        todoSubtask.IsDone = todoSubtask.IsDone;

                        await todoSubtaskRepository.UpdateAsync(todoSubtask, updatedTodoSubtask);
                    }
                }
                // add
                else
                {
                    bool isExistedTodoSubtask = await todoSubtaskRepository.FindAnyByNameAsync(updatedTodoSubtask.Name);

                    if (isExistedTodoSubtask)
                    {
                        return new APIResponse()
                        {
                            Success = false,
                            Message = "There are duplicated Todo Subtasks."
                        };
                    }

                    await todoSubtaskRepository.AddAsync(new TodoSubtask()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = updatedTodoSubtask.Name,
                        IsDone = updatedTodoSubtask.IsDone ?? false,
                        TodoTaskId = todoTaskUpdateDto.Id
                    });
                }
            }

            // delete 
            var existedTodoTask = await todoTaskRepository.GetDetailsAsync(todoTaskUpdateDto.Id);
            var removedTodoSubtasks = existedTodoTask!.TodoSubTasks.Where(u1 => !todoTaskUpdateDto.TodoSubtasks.Any(u2 => u1.Name == u2.Name)).ToList();

            foreach (var removedTodoSubtask in removedTodoSubtasks)
            {
                    await todoSubtaskRepository.DeleteAsync(new TodoSubtask()
                    {
                        Id = removedTodoSubtask.Id,
                        Name = removedTodoSubtask.Name,
                        IsDone = removedTodoSubtask.IsDone ?? false,
                        TodoTaskId = todoTaskUpdateDto.Id
                    });
            }

            return new APIResponse()
            {
                Success = true,
                Data = await todoTaskRepository.GetDetailsAsync(todoTaskUpdateDto.Id)
            };
        }

        public async Task DeleteAsync(string id)
        {
            var todoTask = await todoTaskRepository.GetByIdAsync(id);

            if (todoTask is not null)
            {
                await todoTaskRepository.DeleteAsync(todoTask);
            }
        }

    }
}