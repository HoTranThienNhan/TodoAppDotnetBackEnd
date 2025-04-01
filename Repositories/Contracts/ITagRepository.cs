using todo_app_backend.DTOs.Tag;
using todo_app_backend.Models;

namespace todo_app_backend.Repositories.Contracts
{
    public interface ITagRepository
    {
        Task<bool> FindAnyByNameAsync(string tagName) {
            throw new NotImplementedException();
        } 

        Task<bool> FindAnyUserTagAsync(string userId, string tagId) {
            throw new NotImplementedException();
        }

        Task AddAsync(Tag tag) {
            throw new NotImplementedException();
        }

        Task AddUserTagAsync(UserTag userTag) {
            throw new NotImplementedException();
        }

        Task<UserAllTagsResponseDto?> GetAllByUserIdAsync(string userId) {
            throw new NotImplementedException();
        }

        Task<Tag?> GetByNameAsync(string tagName) {
            throw new NotImplementedException();
        }
    }
}