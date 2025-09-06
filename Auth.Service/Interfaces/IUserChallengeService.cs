using Auth.Domain.Entities.Homeworks;
using Auth.Domain.Entities.Subscriptions;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.UserChallenges;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IUserChallengeService
{
    Task<UserChallengeForViewDto> CompleteChallengeAsync(UserChallengeForCreationDto dto);
    Task<IEnumerable<UserChallengeForViewDto>> GetAllAsync(Expression<Func<UserChallenge, bool>> filter = null, string[] includes = null);
    Task<UserChallengeForViewDto> GetByIdAsync(Expression<Func<UserChallenge, bool>> filter, string[] includes = null);
}
