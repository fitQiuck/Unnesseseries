using Auth.Service.DTOs.Homeworks.HomeworksDto;
using Auth.Service.DTOs.UserChallenges;
using Auth.Service.Interfaces;
using Auth.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserChallengeController : Controller
{
    private readonly IUserChallengeService userChallengeService;

    public UserChallengeController(IUserChallengeService userChallengeService)
    {
        this.userChallengeService = userChallengeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await userChallengeService.GetAllAsync();
        return Ok(result);
    }

    // GET: api/Course/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await userChallengeService.GetAllAsync(c => c.Id == id);
        return Ok(result);
    }

    // POST: api/Course
    [HttpPost]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> CreateAsync([FromBody] UserChallengeForCreationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await userChallengeService.CompleteChallengeAsync(dto);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
    }
}
