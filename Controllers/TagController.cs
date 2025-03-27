using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.DTOs.Tag;
using todo_app_backend.Repositories;
using todo_app_backend.Services;

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

            if (foundTag is not null) {
                return BadRequest(foundTag);
            }

            var tag = await tagService.AddAsync(tagAddDto);

            return Ok(tag);
        }
    }
}