using todo_app_backend.DTOs.TodoSubtask;
using todo_app_backend.Models;

namespace todo_app_backend.Repositories.Contracts
{
    public interface ITodoSubtaskRepository
    {
        Task AddAsync(TodoSubtask todoSubtask) {
            throw new NotImplementedException();
        } 

        Task<TodoSubtask?> GetByIdAsync(string id) {
            throw new NotImplementedException();
        }

        Task UpdateAsync(TodoSubtask todoSubtask, TodoSubtaskUpdateDto todoSubtaskUpdateDto) {
            throw new NotImplementedException();
        } 
    }
}