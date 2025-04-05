using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.DTOs.TodoSubtask;
using todo_app_backend.Models;
using todo_app_backend.Services.Contracts;

namespace todo_app_backend.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [Authorize]

    public class TodoSubtaskController(ITodoSubtaskService todoSubtaskService, ITodoTaskService todoTaskService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult> AddTodoSubtask([FromBody] TodoSubtaskAddDto todoSubtaskAddDto) {
            if (!await todoTaskService.FindAnyAsync(todoSubtaskAddDto.TodoTaskId)) {
                return BadRequest("Todo task Id does not exist.");
            }

            var todoSubtask = await todoSubtaskService.AddAsync(todoSubtaskAddDto);

            return Ok(todoSubtask);
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateTodoSubtask([FromBody] TodoSubtaskDto todoSubtaskUpdateDto) {
            var todoSubtask = await todoSubtaskService.UpdateAsync(todoSubtaskUpdateDto);

            if (todoSubtask!.Success == false) {
                return BadRequest(todoSubtask);
            }

            return Ok(todoSubtask);
        }
    }
}