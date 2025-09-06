using Auth.Service.DTOs.Roles;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IRoleService roleService, ILogger<RoleController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var userId = HttpContextHelper.UserId;
        _logger.LogInformation("User {UserId} requested all roles", userId);

        var hasPermission = HttpContextHelper.UserPermission.FirstOrDefault(item => item == "Role_Get");

        if (hasPermission is not null)
        {
            var allRoles = await _roleService.GetAllAsync();
            return Ok(allRoles);
        }

        return BadRequest("Don't have permission.");
    }

    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetById(Guid roleId)
    {
        var userId = HttpContextHelper.UserId;
        _logger.LogInformation("User {UserId} requested role with ID: {RoleId}", userId, roleId);

        var hasPermission = HttpContextHelper.UserPermission.FirstOrDefault(item => item == "Role_Get");

        if (hasPermission is not null)
        {
            var role = await _roleService.GetAsync(role => role.Id == roleId);
            if (role == null)
            {
                return NotFound($"Role with ID {roleId} not found.");
            }
            return Ok(role);
        }

        return BadRequest("Don't have permission.");
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(RoleForCreationDto roleForCreationDto)
    {
        var userId = HttpContextHelper.UserId;
        _logger.LogInformation("User {UserId} is creating a new role", userId);

        var hasPermission = HttpContextHelper.UserPermission.FirstOrDefault(item => item == "Role_Create");

        if (hasPermission is not null)
        {
            var roleDto = await _roleService.CreateAsync(roleForCreationDto);
            return Ok(roleDto);
        }

        return BadRequest("Don't have permission.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(Guid roleId)
    {
        var userId = HttpContextHelper.UserId;
        _logger.LogInformation("User {UserId} is deleting role with ID: {RoleId}", userId, roleId);

        var hasPermission = HttpContextHelper.UserPermission.FirstOrDefault(item => item == "Role_Delete");

        if (hasPermission is not null)
        {
            var isDeleted = await _roleService.DeleteAsync(role => role.Id == roleId);
            if (!isDeleted)
            {
                return NotFound($"Role with ID {roleId} not found.");
            }
            return NoContent();
        }

        return BadRequest("Don't have permission.");
    }

    [HttpPut("{roleId}")]
    public async Task<IActionResult> UpdateAsync(Guid roleId, [FromBody] RoleForUpdateDto roleForUpdateDto)
    {
        var userId = HttpContextHelper.UserId;
        _logger.LogInformation("User {UserId} is updating role with ID: {RoleId}", userId, roleId);

        var hasPermission = HttpContextHelper.UserPermission.FirstOrDefault(item => item == "Role_Update");

        if (hasPermission is not null)
        {
            if (roleForUpdateDto == null)
            {
                return BadRequest("Role data is required.");
            }

            var updatedRole = await _roleService.UpdateAsync(roleId, roleForUpdateDto);
            if (updatedRole == null)
            {
                return NotFound($"Role with ID {roleId} not found.");
            }

            return Ok(updatedRole);
        }

        return BadRequest("Don't have permission.");
    }
}
