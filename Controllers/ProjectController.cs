using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DTOs;
using ProjectManagement.Repositories;
using System.Security.Claims;

namespace ProjectManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/project")]
    public class ProjectController(PMDbContext db): ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> Create(CreateProjectDto createProjectDTO) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var owner = await db.Users.SingleAsync(x => x.Id == int.Parse(userId));
            var project = new Models.Project
            {
                Name = createProjectDTO.Name,
                CreatedAt = DateTime.UtcNow,
                Owner = owner
            };

            await db.Projects.AddAsync(project);

            await db.SaveChangesAsync();

            return Created();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetOwnerProjects()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var projects = await db.Projects.Include(x => x.Owner).Where(x => x.Owner.Id.ToString() == userId).OrderByDescending(x => x.CreatedAt).ToListAsync();

            var result = projects.Select(x => new
            {
                x.Id,
                x.Name,
                x.CreatedAt
            });

            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var project = await db.Projects.SingleOrDefaultAsync(x => x.Id == projectId);

            if (project == null)
                return NotFound($"Project with id:{projectId} not found!");

            if (project.Owner.Id.ToString() != userId)
                return Unauthorized();

            db.Projects.Remove(project);

            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePartial(int id, UpdateProjectDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var project = await db.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            if (project.Owner.Id.ToString() != userId)
                return Unauthorized();

            if (dto.Name != null)
                project.Name = dto.Name;

            if (dto.OwnerId != null)
            {
                var newOwner = await db.Users.SingleOrDefaultAsync(x => x.Id == dto.OwnerId);
                if (newOwner == null)
                    return BadRequest($"No user found for id:{dto.OwnerId}");

                project.Owner = newOwner;
            }

            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}