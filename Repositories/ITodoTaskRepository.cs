using todo_app_backend.DTOs.TodoTask;
using todo_app_backend.Models;

namespace todo_app_backend.Repositories
{
    public interface ITodoTaskRepository
    {
        Task<bool> FindAnyAsync(string id) {
            throw new NotImplementedException();
        }

        Task<TodoTask?> AddAsync(TodoTaskAddDto todoTaskAddDto) {
            throw new NotImplementedException();
        }

        Task<List<TodoTaskResponseDto>> GetAllWithFilterAndSearchByUserIdAsync(string? userId, string? filter, string? search, bool? isDeleted) {
            throw new NotImplementedException();
        }

        Task<TodoTaskResponseDto?> GetDetailsAsync(string id) {
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