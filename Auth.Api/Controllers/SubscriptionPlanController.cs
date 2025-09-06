using Auth.Service.DTOs.SubscriptionPlans;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionPlanController : Controller
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        /// <summary>
        /// Get all subscription plans.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionPlanForViewDto>>> GetAllAsync()
        {
            var plans = await _subscriptionPlanService.GetAllAsync();
            return Ok(plans);
        }

        /// <summary>
        /// Get a subscription plan by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SubscriptionPlanForViewDto>> GetByIdAsync(Guid id)
        {
            var plan = await _subscriptionPlanService.GetAsync(p => p.Id == id);
            return Ok(plan);
        }

        /// <summary>
        /// Create a new subscription plan.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<SubscriptionPlanForViewDto>> CreateAsync([FromBody] SubscriptionPlanForCreationDto dto)
        {
            var created = await _subscriptionPlanService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update an existing subscription plan.
        /// </summary>
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<SubscriptionPlanForViewDto>> UpdateAsync(Guid id, [FromBody] SubscriptionPlanForUpdateDto dto)
        {
            var updated = await _subscriptionPlanService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        /// <summary>
        /// Delete a subscription plan by ID.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<bool>> DeleteAsync(Guid id)
        {
            var deleted = await _subscriptionPlanService.DeleteAsync(p => p.Id == id);
            return Ok(deleted);
        }
    }
}
