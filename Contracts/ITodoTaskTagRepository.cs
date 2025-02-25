using todo_app_backend.DTOs.TodoTaskTag;
using todo_app_backend.Models;

namespace todo_app_backend.Contracts
{
    public interface ITodoTaskTagRepository
    {
        Task<bool> FindAnyAsync(TodoTaskTagAddDto todoTaskTagAddDto) {
            throw new NotImplementedException();
        }

        Task<TodoTaskTag> AddAsync(TodoTaskTagAddDto todoTaskTagAddDto) {
            throw new NotImplementedException();
        }

        Task<TodoTaskTag?> GetAllTagsByUserId(string userId) {
            throw new NotImplementedException();
        }
    }
}