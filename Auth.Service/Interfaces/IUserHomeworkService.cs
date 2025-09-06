using Auth.Domain.Entities.Homeworks;
using Auth.Service.DTOs.Homeworks.UserHomeworksDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IUserHomeworkService
{
    Task<IEnumerable<UserHomeworkForViewDto>> GetAllAsync(Expression<Func<UserHomework, bool>> filter = null, string[] includes = null);
    Task<UserHomeworkForViewDto> GetAsync(Expression<Func<UserHomework, bool>> filter, string[] includes = null);
    Task<UserHomeworkForViewDto> CreateAsync(UserHomeworkForCreationDto dto);
    Task<UserHomeworkForViewDto> UpdateAsync(Guid id, UserHomeworkForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<UserHomework, bool>> filter);
    Task<UserHomeworkForViewDto> CompleteHomeworkAsync(UserHomeworkForCreationDto dto);

}
