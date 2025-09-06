using Auth.Service.DTOs.Tests.MockTestsDto;
using Auth.Service.Interfaces;
using Auth.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MockTestController : Controller
    {
        private readonly IMockTestService _mockTestService;

        public MockTestController(IMockTestService mockTestService)
        {
            _mockTestService = mockTestService;
        }

        // <summary>
        /// Get all mock tests.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MockTestForViewDto>>> GetAllAsync()
        {
            var result = await _mockTestService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get a specific mock test by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MockTestForViewDto>> GetByIdAsync(Guid id)
        {
            var result = await _mockTestService.GetAsync(m => m.Id == id);
            return Ok(result);
        }

        /// <summary>
        /// Create a new mock test.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MockTestForViewDto>> CreateAsync([FromBody] MockTestForCreationDto dto)
        {
            var result = await _mockTestService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update a mock test (PATCH).
        /// </summary>
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<MockTestForViewDto>> UpdateAsync(Guid id, [FromBody] MockTestForUpdateDto dto)
        {
            var result = await _mockTestService.UpdateAsync(id, dto);
            return Ok(result);
        }

        /// <summary>
        /// Delete a mock test by ID.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<bool>> DeleteAsync(Guid id)
        {
            var result = await _mockTestService.DeleteAsync(m => m.Id == id);
            if (!result)
                return NotFound($"MockTest with ID {id} not found.");

            return Ok(true);
        }

        [HttpPost("{testId}/attach-material")]
        public async Task<IActionResult> AttachMaterial(Guid testId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File cannot be empty");

            var result = await _mockTestService.AttachMaterialAsync(testId, file);
            return Ok(result);
        }

    }
}
