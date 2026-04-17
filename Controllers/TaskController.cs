using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DTOs;
using ProjectManagement.Models;
using ProjectManagement.Repositories;

namespace ProjectManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/task")]
    public class TaskController(PMDbContext db) : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> Create(CreateTaskDto createTask)
        {
            var project = await db.Projects.SingleOrDefaultAsync(x => x.Id == createTask.ProjectId);

            if (project == null)
                return NotFound($"Project with id:{createTask.ProjectId} not found!");

            User? user = null;

            if (createTask.UserId != null)
            {
                user = await db.Users.SingleOrDefaultAsync(x => x.Id == createTask.UserId);

                if (user == null)
                    return NotFound($"User with id:{createTask.UserId} not found!");
            }

            var task = new Models.Task
            {
                Name = createTask.Name,
                Points = createTask.Points,
                Project = project,
                User = user,
                CreatedAt = DateTime.UtcNow,
                Status = Models.Enums.TaskStatus.Pending
            };

            await db.Tasks.AddAsync(task);
            await db.SaveChangesAsync();

            return Created();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await db.Tasks.SingleOrDefaultAsync(x => x.Id == id);
            if (task == null)
                return NotFound($"Task with id:{id} not found!");

            var result = new
            {
                task.Name,
                task.Points,
                task.ProjectId,
                task.UserId,
                task.CreatedAt,
                task.Status
            };

            return Ok(result);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectsTasks(int projectId)
        {
            var tasks = await db.Tasks.Where(x => x.ProjectId == projectId).ToListAsync();

            var result = tasks.Select(x => new
            {
                x.Name,
                x.Points,
                x.CreatedAt,
                x.UserId,
                x.Status
            });

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePartial(int id, UpdateTaskDto updateTask)
        {
            var task = await db.Tasks.SingleOrDefaultAsync(x => x.Id == id);
            if (task == null)
                return NotFound($"Task with id:{id} not found!");

            User? user = null;
            if (updateTask.UserId != null)
            {
                user = await db.Users.SingleOrDefaultAsync(x => x.Id == updateTask.UserId);

                if (user == null)
                    return NotFound($"User with id:{updateTask.UserId} not found!");
            }

            task.Name = updateTask.Name ?? task.Name;
            task.User = user ?? task.User;
            task.Points = updateTask.Points ?? task.Points;
            task.Status = updateTask.Status ?? task.Status;

            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}
