using todo_app_backend.DTOs.TodoTask;
using todo_app_backend.Helpers;

namespace todo_app_backend.Services.Contracts
{
    public interface ITodoTaskService
    {
        Task<bool> FindAnyAsync(string id) {
            throw new NotImplementedException();
        }

        Task<APIResponse?> AddAsync(TodoTaskAddDto todoTaskAddDto) {
            throw new NotImplementedException();
        }

        Task<APIResponse?> GetAllWithFilterAndSearchByUserIdAsync(string? userId, string? filter, string? search, bool? isDeleted) {
            throw new NotImplementedException();
        }

        Task<APIResponse?> GetDetailsAsync(string id) {
            throw new NotImplementedException();
        }

        Task<APIResponse?> UpdateAsync(TodoTaskUpdateDto todoTaskUpdateDto) {
            throw new NotImplementedException();
        }

        Task DeleteAsync(string id) {
            throw new NotImplementedException();
        }
    }
}