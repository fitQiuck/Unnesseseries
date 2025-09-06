using Auth.Domain.Entities.Permissions;
using Auth.Service.DTOs.Permissions;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IPermissionService
{
    Task<Dictionary<string, Dictionary<string, bool>>> GetAllAsync(Expression<Func<Permission, bool>> filter = null, string[] includes = null);
    Task<object> GetAsync(Expression<Func<Permission, bool>> filter = null, string[] includes = null);
    Task<PermissionForViewDto> CreateAsync(PermissionForCreateDto dto);
    Task<bool> DeleteAsync(Expression<Func<Permission, bool>> filter);
    Task<PermissionForViewDto> UpdateAsync(Guid id, PermissionForUpdateDto dto);
    Task<Dictionary<string, Dictionary<string, bool>>> GetPermissionsAsync(List<string> allowedPermissions);
}
