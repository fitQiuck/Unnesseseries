using Auth.Domain.Configurations;
using Auth.Domain.Entities.Roles;
using Auth.Service.DTOs.Roles;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IRoleService
{
    Task<List<RoleForViewDto>> GetAllAsync(Expression<Func<Role, bool>> filter = null, string[] includes = null);
    Task<RoleForViewGetDto> GetAsync(Expression<Func<Role, bool>> filter = null, string[] includes = null);
    Task<RoleForViewDto> CreateAsync(RoleForCreationDto dto);
    Task<bool> DeleteAsync(Expression<Func<Role, bool>> filter);
    Task<RoleForViewDto> UpdateAsync(Guid id, RoleForUpdateDto dto);
}
