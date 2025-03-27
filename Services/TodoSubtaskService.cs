using todo_app_backend.DTOs.TodoSubtask;
using todo_app_backend.Helpers;
using todo_app_backend.Models;
using todo_app_backend.Repositories.Contracts;
using todo_app_backend.Services.Contracts;

namespace todo_app_backend.Services
{
    public class TodoSubtaskService(ITodoSubtaskRepository todoSubtaskRepository) : ITodoSubtaskService
    {
        public async Task<APIResponse?> AddAsync(TodoSubtaskAddDto todoSubtaskAddDto)
        {
            foreach (var todoSubtaskAdd in todoSubtaskAddDto.TodoSubtasks)
            {
                var todoSubtask = new TodoSubtask()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = todoSubtaskAdd.Name,
                    IsDone = todoSubtaskAdd.IsDone ?? false,
                    TodoTaskId = todoSubtaskAddDto.TodoTaskId
                };

                await todoSubtaskRepository.AddAsync(todoSubtask);
            }

            return new APIResponse()
            {
                Success = true,
                Data = todoSubtaskAddDto
            };
        }

        public async Task<APIResponse?> UpdateAsync(TodoSubtaskUpdateDto todoSubtaskUpdateDto)
        {
            var todoSubtask = await todoSubtaskRepository.GetByIdAsync(todoSubtaskUpdateDto.Id);

            if (todoSubtask is null)
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "Todo subtask does not exist."
                };
            }

            await todoSubtaskRepository.UpdateAsync(todoSubtask, todoSubtaskUpdateDto);

            return new APIResponse()
            {
                Success = false,
                Data = todoSubtask
            };
        }
    }
}