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

            var foundTag = await tagService.FindAnyByNameAsync(tagAddDto);

            if (foundTag) {
                return BadRequest("Tag has already existed.");
            }

            var tag = await tagService.AddAsync(tagAddDto);

            return Ok(tag);
        }
    }
}