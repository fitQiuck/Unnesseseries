using Auth.Service.DTOs.Courses.CoursesDto;
using Auth.Service.DTOs.Homeworks.HomeworksDto;
using Auth.Service.Interfaces;
using Auth.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeworkController : Controller
    {
        private readonly IHomeworkService homeworkService;

        public HomeworkController(IHomeworkService homeworkService)
        {
            this.homeworkService = homeworkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await homeworkService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/Course/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await homeworkService.GetAsync(c => c.Id == id);
            return Ok(result);
        }

        // POST: api/Course
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> CreateAsync([FromForm] HomeworkForCreationDto dto)
        {
            var result = await homeworkService.CreateAsync(dto);
            return Ok(result);
        }


        // PUT: api/Course/{id}
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] HomeworkForUpdateDto dto)
        {
            var result = await homeworkService.UpdateAsync(id, dto);
            return Ok(result);
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var isDeleted = await homeworkService.DeleteAsync(c => c.Id == id);
            return Ok(isDeleted);
        }
    }
}
