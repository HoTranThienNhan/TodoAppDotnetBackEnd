using todo_app_backend.DTOs.Tag;
using todo_app_backend.Models;

namespace todo_app_backend.Contracts
{
    public interface ITagRepository
    {
        Task<bool> FindAnyByNameAsync(TagAddDto tagAddDto) {
            throw new NotImplementedException();
        } 

        Task<Tag?> AddAsync(TagAddDto tagAddDto) {
            throw new NotImplementedException();
        }
    }
}