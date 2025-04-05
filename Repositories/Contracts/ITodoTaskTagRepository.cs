using todo_app_backend.Models;

namespace todo_app_backend.Repositories.Contracts
{
    public interface ITodoTaskTagRepository
    {
        Task<List<TodoTaskTag>> GetByTodoTaskId(string todoTaskId) {
            throw new NotImplementedException();
        }
        
        Task AddAsync(TodoTaskTag todoTaskTag) {
            throw new NotImplementedException();
        }

        Task DeleteAsync(List<TodoTaskTag> todoTaskTags) {
            throw new NotImplementedException();
        }
    }
}