using Auth.Domain.Entities.Gamification;
using Auth.Service.DTOs.Gamification.StreakLogDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IStreakLogService
{
    Task<IEnumerable<StreakLogForViewDto>> GetAllAsync(Expression<Func<StreakLog, bool>> filter = null, string[] includes = null);
    Task<StreakLogForViewDto> GetAsync(Expression<Func<StreakLog, bool>> filter, string[] includes = null);
    Task<StreakLogForViewDto> CreateAsync(StreakLogForCreationDto dto);
    Task<StreakLogForViewDto> UpdateAsync(Guid id, StreakLogForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<StreakLog, bool>> filter);


    // Custom addition:
    Task<IEnumerable<StreakLogForViewDto>> GetUserStreakLogAsync(Guid userId);
}
