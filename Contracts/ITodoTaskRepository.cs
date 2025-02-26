using todo_app_backend.DTOs.TodoTask;
using todo_app_backend.Models;

namespace todo_app_backend.Contracts
{
    public interface ITodoTaskRepository
    {
        Task<bool> FindAnyAsync(string id) {
            throw new NotImplementedException();
        }

        Task<TodoTask?> AddAsync(TodoTaskAddDto todoTaskAddDto) {
            throw new NotImplementedException();
        }

        Task<List<TodoTaskResponseDto>> GetAllWithFilterByUserIdAsync(string userId, string? filter, bool? isDeleted) {
            throw new NotImplementedException();
        }

        Task<TodoTaskResponseDto?> GetDetailsWithSearchAsync(string? id, string? search) {
            throw new NotImplementedException();
        }

        Task<TodoTask?> UpdateAsync(TodoTaskUpdateDto todoTaskUpdateDto) {
            throw new NotImplementedException();
        }

        Task DeleteAsync(string id) {
            throw new NotImplementedException();
        }
    }
}