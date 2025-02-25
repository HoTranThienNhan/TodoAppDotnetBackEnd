using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.Contracts;
using todo_app_backend.DTOs.Tag;

namespace todo_app_backend.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [Authorize]

    public class TagController(ITagRepository tagRepository) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult> AddTag(TagAddDto tagAddDto) {

            if (await tagRepository.FindAnyByNameAsync(tagAddDto)) {
                return BadRequest("Tag is already existed!");
            }

            var tag = await tagRepository.AddAsync(tagAddDto);

            return Ok(tag);
        }
    }
}