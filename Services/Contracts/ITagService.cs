using todo_app_backend.DTOs.Tag;
using todo_app_backend.Helpers;

namespace todo_app_backend.Services.Contracts
{
    public interface ITagService
    {
        Task<APIResponse?> AddAsync(TagAddDto tagAddDto) {
            throw new NotImplementedException();
        }

        Task<bool> FindAnyByNameAsync(TagAddDto tagAddDto) {
            throw new NotImplementedException();
        }  

        Task<APIResponse?> GetAllTagsByUserIdAsync(string userId) {
            throw new NotImplementedException();
        }
    }
}