using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.DTOs.Tag;
using todo_app_backend.Services.Contracts;

namespace todo_app_backend.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [Authorize]

    public class TagController(ITagService tagService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult> AddTag(TagAddDto tagAddDto) {

            var tag = await tagService.AddAsync(tagAddDto);

            if (!tag!.Success) {
                return BadRequest(tag);
            }

            return Ok(tag);
        }

        [HttpGet("getAll")]
        public async Task<ActionResult> GetAllByUserId([FromQuery]string userId) {

            var userTags = await tagService.GetAllTagsByUserIdAsync(userId);

            return Ok(userTags);
        }
    }
}