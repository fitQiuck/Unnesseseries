using Auth.Service.DTOs.Tests.MockTestResultsDto;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TestResultController : Controller
    {
        private readonly ITestResultService _testResultService;

        public TestResultController(ITestResultService _testResultService)
        {
            this._testResultService = _testResultService;
        }

        // <summary>
        /// Get all test results.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultForViewDto>>> GetAllAsync()
        {
            var results = await _testResultService.GetAllAsync();
            return Ok(results);
        }

        /// <summary>
        /// Get a test result by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResultForViewDto>> GetByIdAsync(Guid id)
        {
            var result = await _testResultService.GetAsync(r => r.Id == id);
            return Ok(result);
        }

        /// <summary>
        /// Get test results by user ID.
        /// </summary>
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<ResultForViewDto>>> GetByUserIdAsync(Guid userId)
        {
            var results = await _testResultService.GetByUserIdAsync(userId);
            return Ok(results);
        }

        /// <summary>
        /// Get test results by mock test ID.
        /// </summary>
        [HttpGet("mocktest/{mockTestId:guid}")]
        public async Task<ActionResult<IEnumerable<ResultForViewDto>>> GetByMockTestIdAsync(Guid mockTestId)
        {
            var results = await _testResultService.GetByMockTestIdAsync(mockTestId);
            return Ok(results);
        }

        /// <summary>
        /// Calculate average score for a user.
        /// </summary>
        [HttpGet("user/{userId:guid}/average-score")]
        public async Task<ActionResult<double>> CalculateAverageScoreAsync(Guid userId)
        {
            var avg = await _testResultService.CalculateAverageScoreAsync(userId);
            return Ok(avg);
        }

        /// <summary>
        /// Update a test result (PATCH).
        /// </summary>
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<ResultForViewDto>> UpdateAsync(Guid id, [FromBody] ResultForUpdateDto dto)
        {
            var result = await _testResultService.UpdateAsync(id, dto);
            return Ok(result);
        }

        /// <summary>
        /// Delete a test result by ID.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<bool>> DeleteAsync(Guid id)
        {
            var deleted = await _testResultService.DeleteAsync(r => r.Id == id);
            return Ok(deleted);
        }
    }
}
