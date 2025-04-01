using todo_app_backend.DTOs.Tag;
using todo_app_backend.Helpers;
using todo_app_backend.Models;
using todo_app_backend.Repositories.Contracts;
using todo_app_backend.Services.Contracts;

namespace todo_app_backend.Services
{
    public class TagService(ITagRepository tagRepository, IAuthRepository authRepository) : ITagService
    {
        public async Task<APIResponse?> AddAsync(TagAddDto tagAddDto)
        {
            if (!await authRepository.FindAnyByIdAsync(tagAddDto.UserId))
            {
                return new APIResponse()
                {
                    Success = false,
                    Message = "User does not exist."
                };
            }

            Tag? existedTag = await tagRepository.GetByNameAsync(tagAddDto.Name);
            Tag tag = new Tag();
            UserTag userTag = new UserTag();

            if (existedTag is null)
            {
                tag.Id = Guid.NewGuid().ToString();
                tag.Name = tagAddDto.Name;

                userTag.UserId = tagAddDto.UserId;
                userTag.TagId = tag.Id;

                await tagRepository.AddAsync(tag);
                await tagRepository.AddUserTagAsync(userTag);

                return new APIResponse()
                {
                    Success = true,
                    Data = tag
                };
            }
            else
            {

                if (await tagRepository.FindAnyUserTagAsync(tagAddDto.UserId, existedTag.Id))
                {
                    return new APIResponse()
                    {
                        Success = false,
                        Message = "Tag has already existed."
                    };
                }

                userTag.UserId = tagAddDto.UserId;
                userTag.TagId = existedTag.Id;

                await tagRepository.AddUserTagAsync(userTag);

                return new APIResponse()
                {
                    Success = true,
                    Data = existedTag
                };
            }
        }

        public async Task<bool> FindAnyByNameAsync(TagAddDto tagAddDto)
        {
            if (await tagRepository.FindAnyByNameAsync(tagAddDto.Name))
            {
                return true;
            }

            return false;
        }

        public async Task<APIResponse?> GetAllTagsByUserIdAsync(string userId)
        {
            UserAllTagsResponseDto? userTags = await tagRepository.GetAllByUserIdAsync(userId);

            return new APIResponse()
            {
                Success = true,
                Data = userTags
            };
        }
    }
}