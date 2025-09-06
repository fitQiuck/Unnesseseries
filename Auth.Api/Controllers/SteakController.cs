using Auth.Service.DTOs.Gamification.StreaksDto;
using Auth.Service.Interfaces;
using Auth.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SteakController : Controller
    {
        private readonly IStreakService streakService;

        public SteakController(IStreakService streakService)
        {
            this.streakService = streakService;
        }

        /// <summary>
        /// Get streak information by user ID.
        /// </summary>
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<StreakForViewDto>> GetByUserIdAsync(Guid userId)
        {
            var result = await streakService.GetByUserIdAsync(userId);
            if (result is null)
                return NotFound($"Streak for user {userId} not found.");

            return Ok(result);
        }

        /// <summary>
        /// Create a new streak.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<StreakForViewDto>> CreateAsync([FromBody] StreakForCreationDto dto)
        {
            var result = await streakService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByUserIdAsync), new { userId = result.UserId }, result);
        }

        /// <summary>
        /// Update streak information (PATCH).
        /// </summary>
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<StreakForViewDto>> UpdateAsync(Guid id, [FromBody] StreakForUpdateDto dto)
        {
            var result = await streakService.UpdateAsync(id, dto);
            if (result is null)
                return NotFound($"Streak with ID {id} not found.");

            return Ok(result);
        }
    }
}
