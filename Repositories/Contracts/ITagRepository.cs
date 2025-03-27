using todo_app_backend.DTOs.Tag;
using todo_app_backend.Models;

namespace todo_app_backend.Repositories.Contracts
{
    public interface ITagRepository
    {
        Task<bool> FindAnyByNameAsync(TagAddDto tagAddDto) {
            throw new NotImplementedException();
        } 

        Task AddAsync(Tag tag) {
            throw new NotImplementedException();
        }
    }
}