using Auth.Service.DTOs.Gamification.StreaksDto;

namespace Auth.Service.Interfaces;

public interface IStreakService
{
    Task<StreakForViewDto> GetByUserIdAsync(Guid userId);
    Task<StreakForViewDto> CreateAsync(StreakForCreationDto dto);
    Task<StreakForViewDto> UpdateAsync(Guid id, StreakForUpdateDto dto);
}
