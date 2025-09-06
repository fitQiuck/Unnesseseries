using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Permissions;
using Auth.Service.DTOs.Permissions;
using Auth.Service.Exceptions;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class PermissionService : IPermissionService
{
    private readonly IGenericRepository<Permission> _repository;
    private readonly IMapper _mapper;

    public PermissionService(IGenericRepository<Permission> _repository, IMapper _mapper)
    {
        this._repository = _repository;
        this._mapper = _mapper;
    }

    public async Task<Dictionary<string, Dictionary<string, bool>>> GetAllAsync(Expression<Func<Permission, bool>> filter = null, string[] includes = null)
    {
        // Fetch all permissions from the repository
        var permissions = await _repository.GetAll(filter, includes).ToListAsync();

        // Group permissions and map them into a dictionary structure
        var groupedPermissions = permissions
            .Select(p => p.Name)
            .GroupBy(p => p.Split('_')[0])
            .ToDictionary(
                group => group.Key.ToLower(),
                group => group
                    .GroupBy(permission => permission.Split('_')[1])
                    .ToDictionary(
                        permissionGroup => permissionGroup.Key,
                        permissionGroup => true // Assuming all permissions are valid, set to true
                    )
            );

        return groupedPermissions;
    }



    public async Task<object> GetAsync(Expression<Func<Permission, bool>> filter, string[] includes = null)
    {
        var permission = await _repository.GetAll(filter, includes).ToListAsync();
        if (permission == null)
            throw new HttpStatusCodeException(404, "Permission not found");

        var groupedPermissions = permission
           .Select(p => p.Name)
           .GroupBy(p => p.Split('_')[0])
           .ToDictionary(
               group => group.Key.ToLower(),
               group => group
                   .GroupBy(permission => permission.Split('_')[1])
                   .ToDictionary(
                       permissionGroup => permissionGroup.Key,
               permissionGroup => true // Assuming all permissions are valid, set to true
                   )
           );

        return groupedPermissions;
    }

    public async Task<PermissionForViewDto> CreateAsync(PermissionForCreateDto dto)
    {
        var res = await _repository.GetAsync(p => p.Name == dto.Name);
        if (res != null)
            throw new HttpStatusCodeException(400, "Permission already exists");

        var permission = _mapper.Map<Permission>(dto);
        permission = await _repository.CreateAsync(permission);
        await _repository.SaveChangesAsync();

        return _mapper.Map<PermissionForViewDto>(permission);
    }

    public async Task<bool> DeleteAsync(Expression<Func<Permission, bool>> filter)
    {
        var permission = await _repository.GetAsync(filter);
        if (permission == null)
            throw new HttpStatusCodeException(404, "Permission not found");

        // Correct the argument passed to DeleteAsync by using the filter expression  
        await _repository.DeleteAsync(permission);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<PermissionForViewDto> UpdateAsync(Guid id, PermissionForUpdateDto dto)
    {
        var permission = await _repository.GetAsync(p => p.Id == id);
        if (permission == null)
            throw new HttpStatusCodeException(404, "Permission not found");

        permission = _mapper.Map(dto, permission);

        _repository.Update(permission);
        await _repository.SaveChangesAsync();

        return _mapper.Map<PermissionForViewDto>(permission);
    }


    public async Task<Dictionary<string, Dictionary<string, bool>>> GetPermissionsAsync(List<string> allowedPermissions)
    {
        var allPermissions = await _repository.GetAll(null).ToListAsync();

        var groupedPermissions = allPermissions
            .Select(p => p.Name)
            .GroupBy(p => p.Split('_')[0])
            .ToDictionary(
                group => group.Key.ToLower(),
                group => group
                    .GroupBy(permission => permission.Split('_')[1])
                    .ToDictionary(
                        permissionGroup => permissionGroup.Key,
                        permissionGroup => allowedPermissions.Contains(permissionGroup.First())
                    )
            );

        return groupedPermissions;
    }
}
