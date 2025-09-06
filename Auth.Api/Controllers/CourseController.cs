using Auth.Service.DTOs.Courses.CoursesDto;
using Auth.Service.Interfaces;
using Auth.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        // GET: api/Course
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await courseService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/Course/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await courseService.GetAsync(c => c.Id == id);
            if (result is null)
                return NotFound();

            return Ok(result);
        }


        // POST: api/Course
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> CreateAsync([FromBody] CourseForCreationDto dto)
        {
            var result = await courseService.CreateAsync(dto);
            return Ok(result);

        }

        // PUT: api/Course/{id}
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CourseForUpdateDto dto)
        {
            var updatedCourse = await courseService.UpdateAsync(id, dto);
            return Ok(updatedCourse);
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var isDeleted = await courseService.DeleteAsync(c => c.Id == id);
            return Ok(isDeleted);
        }


    }
}
