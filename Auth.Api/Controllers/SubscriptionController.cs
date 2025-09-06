using Auth.Service.DTOs.Subscriptions;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        // <summary>
        /// Get all subscriptions.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionForViewDto>>> GetAllAsync()
        {
            var subscriptions = await _subscriptionService.GetAllAsync();
            return Ok(subscriptions);
        }

        /// <summary>
        /// Get subscription by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SubscriptionForViewDto>> GetByIdAsync(Guid id)
        {
            var subscription = await _subscriptionService.GetAsync(s => s.Id == id);
            return Ok(subscription);
        }

        /// <summary>
        /// Check if a user has an active subscription.
        /// </summary>
        [HttpGet("user/{userId:guid}/is-active")]
        public async Task<ActionResult<bool>> IsSubscriptionActiveAsync(Guid userId)
        {
            var isActive = await _subscriptionService.IsSubscriptionActiveAsync(userId);
            return Ok(isActive);
        }

        /// <summary>
        /// Get active subscription for a user.
        /// </summary>
        [HttpGet("user/{userId:guid}/active")]
        public async Task<ActionResult<SubscriptionForViewDto>> GetActiveSubscriptionAsync(Guid userId)
        {
            var subscription = await _subscriptionService.GetActiveSubscriptionAsync(userId);
            if (subscription == null)
                return NotFound("No active subscription found");

            return Ok(subscription);
        }

        /// <summary>
        /// Create a new subscription.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<SubscriptionForViewDto>> CreateAsync([FromBody] SubscriptionForCreateDto dto)
        {
            var created = await _subscriptionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update a subscription (PATCH).
        /// </summary>
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<SubscriptionForViewDto>> UpdateAsync(Guid id, [FromBody] SubscriptionForUpdateDto dto)
        {
            var updated = await _subscriptionService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        /// <summary>
        /// Delete a subscription by ID.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<bool>> DeleteAsync(Guid id)
        {
            var deleted = await _subscriptionService.DeleteAsync(s => s.Id == id);
            return Ok(deleted);
        }

        /// <summary>
        /// Extend subscription duration.
        /// </summary>
        [HttpPost("{id:guid}/extend")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult> ExtendSubscriptionAsync(Guid id, [FromQuery] long additionalDays)
        {
            await _subscriptionService.ExtendSubscriptionAsync(id, additionalDays);
            return NoContent();
        }
    }
}
