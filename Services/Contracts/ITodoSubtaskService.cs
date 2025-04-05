using todo_app_backend.DTOs.TodoSubtask;
using todo_app_backend.Helpers;

namespace todo_app_backend.Services.Contracts
{
    public interface ITodoSubtaskService
    {
        Task<APIResponse?> AddAsync(TodoSubtaskAddDto todoSubtaskAddDto) {
            throw new NotImplementedException();
        } 

        Task<APIResponse?> UpdateAsync(TodoSubtaskDto todoSubtaskUpdateDto) {
            throw new NotImplementedException();
        } 
    }
}