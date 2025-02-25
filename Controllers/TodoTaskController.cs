using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_app_backend.Contracts;
using todo_app_backend.DTOs.TodoTask;

namespace todo_app_backend.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [Authorize]

    public class TodoTaskController(ITodoTaskRepository todoTaskRepository, IAuthRepository authRepository) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult> AddTodoTask([FromBody] TodoTaskAddDto todoTaskAddDto) {
            if (!await authRepository.FindAnyByIdAsync(todoTaskAddDto.UserId)) {
                return BadRequest("UserID does not exist.");
            }

            var todoTask = await todoTaskRepository.AddAsync(todoTaskAddDto);

            return Ok(todoTask);
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllTodoTaskByUserId([FromQuery] string userId) {
            var todoTasks = await todoTaskRepository.GetAllByUserIdAsync(userId);

            return Ok(todoTasks);
        }

        [HttpGet("details")]
        public async Task<ActionResult> GetTodoTaskDetails([FromQuery] string id) {
            var todoTask = await todoTaskRepository.GetDetailsAsync(id);

            return Ok(todoTask);
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateTodoTask([FromBody] TodoTaskUpdateDto todoTaskUpdateDto) {
            var todoTask = await todoTaskRepository.UpdateAsync(todoTaskUpdateDto);

            if (todoTask is null) {
                return BadRequest("Todo task does not exist.");
            }

            return Ok(todoTask);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteTodoTask([FromQuery] string id) {
            if (!await todoTaskRepository.FindAnyAsync(id)) {
                return BadRequest("Todo task does not exist.");
            }

            await todoTaskRepository.DeleteAsync(id);

            return Ok($"Delete Todo task with id: {id} task successfully.");
        }
    }
}