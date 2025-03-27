using todo_app_backend.DTOs.Tag;
using todo_app_backend.Helpers;
using todo_app_backend.Models;
using todo_app_backend.Repositories;

namespace todo_app_backend.Services
{
    public class TagService(ITagRepository tagRepository): ITagService
    {
        public async Task<APIResponse?> AddAsync(TagAddDto tagAddDto) {
            var tag = new Tag() {
                Id = Guid.NewGuid().ToString(),
                Name = tagAddDto.Name
            };

            await tagRepository.AddAsync(tag);

            return new APIResponse() {
                Success = true,
                Data = tag
            };
        }

        public async Task<APIResponse?> FindAnyByNameAsync(TagAddDto tagAddDto) {
            if (await tagRepository.FindAnyByNameAsync(tagAddDto)) {
                return new APIResponse() {
                    Success = false,
                    Message = "Tag is already existed!"
                };
            } 

            return null;
        }  
    }
}