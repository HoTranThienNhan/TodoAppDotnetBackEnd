using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.Contracts;
using todo_app_backend.DTOs.TodoSubtask;

namespace todo_app_backend.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [Authorize]

    public class TodoSubtaskController(ITodoSubtaskRepository todoSubtaskRepository, ITodoTaskRepository todoTaskRepository) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult> AddTodoSubtask([FromBody] TodoSubtaskAddDto todoSubtaskAddDto) {
            if (!await todoTaskRepository.FindAnyAsync(todoSubtaskAddDto.TodoTaskId)) {
                return BadRequest("Todo task Id does not exist.");
            }

            var todoSubtask = await todoSubtaskRepository.AddAsync(todoSubtaskAddDto);

            return Ok(todoSubtask);
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateTodoSubtask([FromBody] TodoSubtaskUpdateDto todoSubtaskUpdateDto) {
            var todoSubtask = await todoSubtaskRepository.UpdateAsync(todoSubtaskUpdateDto);

            if (todoSubtask is null) {
                return BadRequest("Todo subtask does not exist.");
            }

            return Ok(todoSubtask);
        }
    }
}