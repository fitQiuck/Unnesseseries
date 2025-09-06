using Auth.Service.DTOs.Gamification.StreakLogDto;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StreakLogController : Controller
    {
        private readonly IStreakLogService _streakLogService;

        public StreakLogController(IStreakLogService streakLogService)
        {
            _streakLogService = streakLogService;
        }

        /// <summary>
        /// Get all streak logs (optionally filtered).
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StreakLogForViewDto>>> GetAllAsync()
        {
            var result = await _streakLogService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get a specific streak log by its ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StreakLogForViewDto>> GetByIdAsync(Guid id)
        {
            var result = await _streakLogService.GetAsync(l => l.Id == id);
            return Ok(result);
        }

        /// <summary>
        /// Get all streak logs for a specific user.
        /// </summary>
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<StreakLogForViewDto>>> GetUserStreakLogAsync(Guid userId)
        {
            var result = await _streakLogService.GetUserStreakLogAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Create a new streak log.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<StreakLogForViewDto>> CreateAsync([FromBody] StreakLogForCreationDto dto)
        {
            var result = await _streakLogService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing streak log (PATCH).
        /// </summary>
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<StreakLogForViewDto>> UpdateAsync(Guid id, [FromBody] StreakLogForUpdateDto dto)
        {
            var result = await _streakLogService.UpdateAsync(id, dto);
            return Ok(result);
        }

        /// <summary>
        /// Delete a streak log by its ID.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<bool>> DeleteAsync(Guid id)
        {
            var result = await _streakLogService.DeleteAsync(l => l.Id == id);
            if (!result)
                return NotFound($"Streak log with ID {id} not found.");

            return Ok(true);
        }
    }
}
