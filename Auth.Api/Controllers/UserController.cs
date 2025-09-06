using Auth.Domain.Configurations;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Users;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Auth.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params, [FromQuery] string? search)
        {
            var hasPermission = HttpContextHelper.UserPermission
                .FirstOrDefault(item => item == "User_Get");

            if (hasPermission is null)
                return BadRequest("Don't have permission.");

            // If search is null, set it to an empty string:
            string normalizedSearch = search?.ToLower() ?? "";

            var users = await _userService.GetAllAsync(
                @params,
                item => (item.FullName ?? "")
                    .ToLower()
                    .Contains(normalizedSearch));

            return Ok(users);
        }

        [HttpGet("{userId}"), Authorize]
        public async Task<IActionResult> GetById(Guid userId)
        {
            var currentUserId = HttpContextHelper.UserId;
            _logger.LogInformation("User {UserId} is requesting user by ID: {TargetUserId}", currentUserId, userId);

            var hasPermission = HttpContextHelper.UserPermission.FirstOrDefault(item => item == "User_Get");

            if (hasPermission is not null)
            {
                var user = await _userService.GetAsync(user => user.Id == userId);
                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found.");
                }
                return Ok(user);
            }
            return BadRequest("Don't have permission.");
        }


        [HttpPost]
        public async Task<ActionResult<UserForViewDto>> CreateUser(UserForCreationDto userForCreationDto)
        {
            var userId = HttpContextHelper.UserId;
            _logger.LogInformation("User {UserId} is creating a new user.", userId);

            var hasPermission = HttpContextHelper.UserPermission.FirstOrDefault(item => item == "User_Create");

            if (hasPermission is not null)
            {
                var userDto = await _userService.CreateAsync(userForCreationDto);
                return CreatedAtAction(nameof(CreateUser), new { id = userDto }, userDto);
            }
            return BadRequest("Don't have permission.");
        }

        [HttpPatch("{userId}"), Authorize]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserForUpdateDto userForUpdateDto)
        {
            var currentUserId = HttpContextHelper.UserId;
            _logger.LogInformation("User {UserId} is updating user with ID: {TargetUserId}", currentUserId, userId);

            var hasPermission = HttpContextHelper.UserPermission.FirstOrDefault(item => item == "User_Update");

            if (hasPermission is not null)
            {
                if (userForUpdateDto == null)
                {
                    return BadRequest("User data is required.");
                }
                var updatedUser = await _userService.UpdateAsync(userId, userForUpdateDto);
                if (updatedUser == null)
                {
                    return NotFound($"User with ID {userId} not found.");
                }
                return Ok(updatedUser);
            }
            return BadRequest("Don't have permission.");
        }


        // ...
        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var currentUserId = HttpContextHelper.UserId;
            _logger.LogInformation("User {UserId} is deleting user with ID: {TargetUserId}", currentUserId, userId);

            var hasPermission = HttpContextHelper.UserPermission.FirstOrDefault(item => item == "User_Delete");

            if (hasPermission is not null)
            {
                var isDeleted = await _userService.DeleteAsync(user => user.Id == userId);
                if (!isDeleted)
                {
                    return NotFound($"User with ID {userId} not found.");
                }
                return NoContent();
            }
            return BadRequest("Don't have permission.");
        }


        [HttpPatch("change-password")]
        public async ValueTask<IActionResult> ChangePasswordAsync([FromQuery] string email, [FromQuery] string newPassword)
            => Ok(await _userService.ChangePassword(email, newPassword));


        [HttpPatch("{userId:long}/add-points")]
        public async ValueTask<IActionResult> AddPointsAsync(Guid userId, [FromQuery] int points)
        {
            await _userService.AddPointsAsync(userId, points);
            return Ok();
        }
    }
}
