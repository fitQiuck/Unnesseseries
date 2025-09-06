using Auth.Service.DTOs.Gamification.DailyChallengesDto;
using Auth.Service.DTOs.UserChallenges;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DailyChallengeController : Controller
    {
        private readonly IDaliyChallengeService service;

        public DailyChallengeController(IDaliyChallengeService dailyChallengeService)
        {
            service = dailyChallengeService;
        }

        // GET: api/dailychallenge
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            var challenges = await service.GetAllAsync();
            return Ok(challenges);
        }

        // GET: api/dailychallenge/{id}
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var challenge = await service.GetAsync(c => c.Id == id);
            if (challenge is null)
                return NotFound();

            return Ok(challenge);
        }

        // POST: api/dailychallenge
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> CreateAsync([FromBody] DailyChallengeForCreateDto dto)
        {
            var createdChallenge = await service.CreateAsync(dto);
            return Ok(createdChallenge);
        }

        // PATCH: api/dailychallenge/{id}
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] DailyChallengeForUpdateDto dto)
        {
            var updated = await service.UpdateAsync(id, dto);
            if (updated is null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE: api/dailychallenge/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var deleted = await service.DeleteAsync(c => c.Id == id);
            if (!deleted)
                return NotFound();

            return Ok(deleted);
        }

        // GET: api/dailychallenge/today
        [HttpGet("today")]
        [Authorize]
        public async Task<IActionResult> GetTodayChallengeAsync()
        {
            var todayChallenge = await service.GetTodayChallengeAsync();
            if (todayChallenge is null)
                return NotFound();

            return Ok(todayChallenge);
        }

        // POST: api/dailychallenge/complete
        [HttpPost("complete")]
        [Authorize]
        public async Task<IActionResult> CompleteChallengeAsync([FromBody] UserChallengeForCreationDto dto)
        {
            var result = await service.CompleteChallengeAsync(dto);
            return Ok(result);
        }
    }
}
