using Auth.DataAccess.Interface;
using Auth.Domain.Configurations;
using Auth.Domain.Entities.Permissions;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Roles;
using Auth.Service.Exceptions;
using Auth.Service.Extensions;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Text.Json;

namespace Auth.Service.Services;

public class RoleService : IRoleService
{
    private readonly IGenericRepository<Role> _repository;
    private readonly IGenericRepository<Permission> _permissionRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly ILogger<RoleService> _logger;
    private readonly IPermissionService _permissionService;
    private readonly IMapper _mapper;

    public RoleService(IGenericRepository<Role> repository, IMapper mapper, IGenericRepository<Permission> permissionRepository,
        IPermissionService permissionService, IGenericRepository<User> userRepository, ILogger<RoleService> logger)
    {
        this._repository = repository;
        this._mapper = mapper;
        this._permissionRepository = permissionRepository;
        this._permissionService = permissionService;
        this._userRepository = userRepository;
        this._logger = logger;
    }

    public async Task<List<RoleForViewDto>> GetAllAsync(
        Expression<Func<Role, bool>> filter = null,
        string[] includes = null)
    {
        _logger.LogInformation("Fetching all roles...");

        var roles = _repository.GetAll(filter); //, includes: includes ?? new[] { "RolePermissions", "UserRoles" }
        var rolesList = _mapper.Map<List<RoleForViewDto>>(roles);

        _logger.LogInformation("{Count} roles fetched.", rolesList.Count);
        return rolesList;
    }

    public async Task<RoleForViewGetDto> GetAsync(
        Expression<Func<Role, bool>> filter,
        string[] includes = null)
    {
        _logger.LogInformation("Fetching role by filter...");

        var role = await _repository.GetAsync(filter, includes: includes ?? new[] { "UserRoles", "RolePermissions" });
        if (role == null)
        {
            _logger.LogWarning("Role not found for the provided filter.");
            throw new HttpStatusCodeException(404, "Role not found");
        }

        var userFullNames = role.UserRole.Select(ur => ur.FullName).ToList();
        var createdByUser = await _userRepository.GetAsync(u => u.Id == role.CreatedBy);
        var permissions = await _permissionService.GetPermissionsAsync(
            role.RolePermission.Select(p => p.Name).ToList()
        );

        var result = new RoleForViewGetDto
        {
            Id = role.Id,
            Name = role.Name,
            CreatedAt = role.CreatedAt,
            CreatedBy = createdByUser?.FullName ?? "Unknown",
            Users = userFullNames,
            Permissions = permissions
        };

        _logger.LogInformation("Role fetched successfully: {RoleName}", role.Name);
        return result;
    }

    public async Task<RoleForViewDto> CreateAsync(RoleForCreationDto dto)
    {
        var res = await _repository.GetAsync(item => item.Name == dto.Name);
        if (res != null)
            throw new HttpStatusCodeException(400, "Role already exists");

        List<Permission> permissionsResult = new();

        if (dto.RolePermissions != null)
        {
            foreach (var id in dto.RolePermissions)
            {
                var permission = await _permissionRepository.GetAsync(p => p.Id == id);
                if (permission == null)
                    throw new HttpStatusCodeException(404, $"Permission with Id {id} not found");

                permissionsResult.Add(permission);
            }
        }

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            RolePermission = permissionsResult,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = HttpContextHelper.UserId ?? Guid.Empty // Seederda null bo‘lishi mumkin
        };

        await _repository.CreateAsync(role);
        await _repository.SaveChangesAsync();

        role.RolePermission = null;

        return _mapper.Map<RoleForViewDto>(role);
    }



    public async Task<bool> DeleteAsync(Expression<Func<Role, bool>> filter)
    {
        _logger.LogInformation("Attempting to delete role...");

        var role = await _repository.GetAsync(filter);
        if (role == null)
        {
            _logger.LogWarning("Role not found for deletion.");
            throw new HttpStatusCodeException(404, "Role not found");
        }

        role.DeletedBy = HttpContextHelper.UserId;

        await _repository.DeleteAsync(role);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Role deleted successfully. Role ID: {RoleId}", role.Id);
        return true;
    }

    public async Task<RoleForViewDto> UpdateAsync(Guid id, RoleForUpdateDto dto)
    {
        _logger.LogWarning("UpdateAsync for roles is not yet implemented.");
        // Rolni topish
        var role = await _repository.GetAsync(r => r.Id == id, includes: new[] { "RolePermissions" });
        if (role == null)
            throw new HttpStatusCodeException(404, "Role not found");



        var result = new List<string>();

        if (dto.Permissions is JsonElement jsonElement)
        {
            var permissions = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, bool>>>(jsonElement.GetRawText());
            if (permissions == null)
                throw new ArgumentException("Invalid PermissionId format.");

            foreach (var section in permissions)
            {
                foreach (var action in section.Value)
                {
                    if (action.Value == true)
                    {
                        string key = $"{section.Key}_{action.Key}";
                        result.Add(key.ToString());
                    }
                }
            }
        }
        else
        {
            throw new ArgumentException("Invalid PermissionId format.");
        }

        List<Permission> permissionsResult = new List<Permission>();
        foreach (var Id in result)
        {
            var permission = await _permissionRepository.GetAsync(item => item.Name.ToLower() == Id.ToLower());
            if (permission == null)
                throw new HttpStatusCodeException(404, $"Permission with Id {Id} not found");

            permissionsResult.Add(permission);
        }


        _mapper.Map(dto, role);

        role.RolePermission.Clear();

        role.RolePermission = permissionsResult;

        role.UpdatedBy = HttpContextHelper.UserId;
        role.UpdatedAt = DateTime.UtcNow;

        _repository.Update(role);
        await _repository.SaveChangesAsync();

        role.RolePermission = null;

        return _mapper.Map<RoleForViewDto>(role);
    }
}
