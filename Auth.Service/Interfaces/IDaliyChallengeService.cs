using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Gamification;
using Auth.Service.DTOs.Gamification.DailyChallengesDto;
using Auth.Service.DTOs.UserChallenges;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IDaliyChallengeService
{
    Task<IEnumerable<DailyChallengeForViewDto>> GetAllAsync(Expression<Func<DailyChallengge, bool>> filter = null, string[] includes = null);
    Task<DailyChallengeForViewDto> GetAsync(Expression<Func<DailyChallengge, bool>> filter, string[] includes = null);
    Task<DailyChallengeForViewDto> CreateAsync(DailyChallengeForCreateDto dto);
    Task<DailyChallengeForViewDto> UpdateAsync(Guid id, DailyChallengeForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<DailyChallengge, bool>> filter);
    Task<DailyChallengeForViewDto?> GetTodayChallengeAsync();
    Task<UserChallengeForViewDto> CompleteChallengeAsync(UserChallengeForCreationDto dto);

}
