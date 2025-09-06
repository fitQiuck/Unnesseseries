using Auth.Service.DTOs.Courses.LessonsDto;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ILessonService lessonService;

        public LessonController(ILessonService lessonService)
        {
            this.lessonService = lessonService;
        }

        /// <summary>
        /// Create a new lesson
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> CreateAsync([FromForm] LessonForCreationDto dto)
        {
            var created = await lessonService.CreateAsync(dto);
            return Ok(created);
        }

        /// <summary>
        /// Update a lesson
        /// </summary>
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] LessonForUpdateDto dto)
        {
            var updated = await lessonService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        /// <summary>
        /// Delete a lesson
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await lessonService.DeleteAsync(c => c.Id == id);
            return NoContent();
        }

        /// <summary>
        /// Get a lesson by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var lesson = await lessonService.GetAsync(c => c.Id == id);
            return Ok(lesson);
        }

        /// <summary>
        /// Get all lessons
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var lessons = await lessonService.GetAllAsync();
            return Ok(lessons);
        }
    }
}
