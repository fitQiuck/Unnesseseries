using Auth.Domain.Configurations;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Users;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserForViewDto>> GetAllAsync(PaginationParams @params, Expression<Func<User, bool>> filter = null, string[] includes = null);

    Task<UserForViewDto> GetAsync(Expression<Func<User, bool>> filter, string[] includes = null);

    Task<UserForViewDto> CreateAsync(UserForCreationDto dto);

    Task<bool> DeleteAsync(Expression<Func<User, bool>> filter);

    Task<UserForViewDto> UpdateAsync(Guid id, UserForUpdateDto dto);

    Task<bool> ChangePassword(string email, string password);
    Task AddPointsAsync(Guid userId, int points);
}
