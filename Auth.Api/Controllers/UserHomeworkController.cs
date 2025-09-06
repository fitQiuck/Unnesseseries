using Auth.Service.DTOs.Homeworks.HomeworksDto;
using Auth.Service.DTOs.Homeworks.UserHomeworksDto;
using Auth.Service.Interfaces;
using Auth.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserHomeworkController : Controller
    {
        private readonly IUserHomeworkService userHomeworkService;

        public UserHomeworkController(IUserHomeworkService userHomeworkService)
        {
            this.userHomeworkService = userHomeworkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await userHomeworkService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/Course/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await userHomeworkService.GetAsync(c => c.Id == id);
            return Ok(result);
        }

        // POST: api/Course
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> CreateAsync([FromBody] UserHomeworkForCreationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userHomeworkService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        // PUT: api/Course/{id}
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UserHomeworkForUpdateDto dto)
        {
            var updatedCourse = await userHomeworkService.UpdateAsync(id, dto);
            return Ok(updatedCourse);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var isDeleted = await userHomeworkService.DeleteAsync(c => c.Id == id);
            return Ok(isDeleted);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User, SuperAdmin")]
        public async Task<IActionResult> CompleteHomework(UserHomeworkForCreationDto dto)
        {
            var completedHomework = await userHomeworkService.CompleteHomeworkAsync(dto);
            return Ok(completedHomework);
        }
    }
}
