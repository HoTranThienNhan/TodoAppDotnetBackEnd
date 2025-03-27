using todo_app_backend.DTOs.TodoTask;
using todo_app_backend.Models;

namespace todo_app_backend.Repositories.Contracts
{
    public interface ITodoTaskRepository
    {
        Task<bool> FindAnyAsync(string id) {
            throw new NotImplementedException();
        }

        Task<TodoTask?> GetByIdAsync(string id) {
            throw new NotImplementedException();
        }

        Task AddAsync(TodoTask todoTask) {
            throw new NotImplementedException();
        }

        Task AddTodoTaskTagAsync(TodoTaskTag todoTaskTag) {
            throw new NotImplementedException();
        }

        Task<List<TodoTaskResponseDto>> GetAllWithFilterAndSearchByUserIdAsync(string? userId, string? filter, string? search, bool? isDeleted) {
            throw new NotImplementedException();
        }

        Task<TodoTaskResponseDto?> GetDetailsAsync(string id) {
            throw new NotImplementedException();
        }

        Task<TodoTask?> UpdateAsync(TodoTask todoTask, TodoTaskUpdateDto todoTaskUpdateDto) {
            throw new NotImplementedException();
        }

        Task DeleteAsync(TodoTask todoTask) {
            throw new NotImplementedException();
        }
    }
}