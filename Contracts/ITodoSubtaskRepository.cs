using todo_app_backend.DTOs.TodoSubtask;
using todo_app_backend.Models;

namespace todo_app_backend.Contracts
{
    public interface ITodoSubtaskRepository
    {
        Task<TodoSubtaskAddDto?> AddAsync(TodoSubtaskAddDto todoSubtaskAddDto) {
            throw new NotImplementedException();
        } 

        Task<TodoSubtask?> UpdateAsync(TodoSubtaskUpdateDto todoSubtaskUpdateDto) {
            throw new NotImplementedException();
        } 
    }
}