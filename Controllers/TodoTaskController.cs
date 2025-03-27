using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.DTOs.TodoTask;
using todo_app_backend.Repositories.Contracts;
using todo_app_backend.Services.Contracts;

namespace todo_app_backend.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [Authorize]

    public class TodoTaskController(ITodoTaskService todoTaskService, IAuthRepository authRepository) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult> AddTodoTask([FromBody] TodoTaskAddDto todoTaskAddDto) {
            if (!await authRepository.FindAnyByIdAsync(todoTaskAddDto.UserId)) {
                return BadRequest("UserID does not exist.");
            }

            var todoTask = await todoTaskService.AddAsync(todoTaskAddDto);

            return Ok(todoTask);
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllTodoTasksWithFilterByUserId([FromQuery] string? userId, string? filter, string? search, bool? isDeleted = false) {
            var todoTasks = await todoTaskService.GetAllWithFilterAndSearchByUserIdAsync(userId, filter, search, isDeleted);

            if(todoTasks!.Success == false) {
                return BadRequest(todoTasks);
            }

            return Ok(todoTasks);
        }

        [HttpGet("details")]
        public async Task<ActionResult> GetTodoTaskDetailsWithSearch([FromQuery] string id) {
            var todoTask = await todoTaskService.GetDetailsAsync(id);

            return Ok(todoTask);
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateTodoTask([FromBody] TodoTaskUpdateDto todoTaskUpdateDto) {
            var todoTask = await todoTaskService.UpdateAsync(todoTaskUpdateDto);

            if(todoTask!.Success == false) {
                return BadRequest(todoTask);
            }

            return Ok(todoTask);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteTodoTask([FromQuery] string id) {
            if (!await todoTaskService.FindAnyAsync(id)) {
                return BadRequest("Todo task does not exist.");
            }

            await todoTaskService.DeleteAsync(id);

            return Ok($"Delete Todo task with id: {id} task successfully.");
        }
    }
}