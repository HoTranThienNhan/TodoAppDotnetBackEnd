using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.Contracts;
using todo_app_backend.DTOs.TodoTaskTag;

namespace todo_app_backend.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [Authorize]

    public class TodoTaskTagController(ITodoTaskTagRepository todoTaskTagRepository) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult> AddTodoTaskTag([FromBody] TodoTaskTagAddDto todoTaskTagAddDto) {
            if (await todoTaskTagRepository.FindAnyAsync(todoTaskTagAddDto)) {
                return BadRequest("The Todo Task ID with Tag ID has existed.");
            }

            var todoTaskTag = await todoTaskTagRepository.AddAsync(todoTaskTagAddDto);

            return Ok(todoTaskTag);
        }
    }
}