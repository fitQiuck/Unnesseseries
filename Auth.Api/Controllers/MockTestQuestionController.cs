using Auth.Service.DTOs.Tests.MockTestQuestionsDto;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/mocktests/{mockTestId}/questions")]
    [ApiController]
    public class MockTestQuestionController : ControllerBase
    {
        private readonly IMockTestQuestionService questionService;

        public MockTestQuestionController(IMockTestQuestionService questionService)
        {
            this.questionService = questionService;
        }

        // GET: api/mocktests/{mockTestId}/questions
        [HttpGet]
        [AllowAnonymous] // Students can fetch questions
        public async Task<IActionResult> GetAll(Guid mockTestId)
        {
            var result = await questionService.GetAllAsync(q => q.MockTestId == mockTestId);
            return Ok(result);
        }

        // GET: api/mocktests/{mockTestId}/questions/{id}
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid mockTestId, Guid id)
        {
            var result = await questionService.GetAsync(q => q.MockTestId == mockTestId && q.Id == id);
            return Ok(result);
        }

        // POST: api/mocktests/{mockTestId}/questions
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create(Guid mockTestId, [FromForm] MockTestQuestionForCreationDto dto)
        {

            var created = await questionService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { mockTestId, id = created.Id }, created);
        }

        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] MockTestQuestionForUpdateDto dto)
        {
            var updated = await questionService.UpdateAsync(id, dto);
            return Ok(updated);
        }


        // DELETE: api/mocktests/{mockTestId}/questions/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(Guid mockTestId, Guid id)
        {
            var deleted = await questionService.DeleteAsync(q => q.MockTestId == mockTestId && q.Id == id);
            return Ok(deleted);
        }
    }
}
