using Auth.Domain.Entities.Subscriptions;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Subscriptions;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ISubscriptionService
{
    Task<IEnumerable<SubscriptionForViewDto>> GetAllAsync(Expression<Func<Subscription, bool>> filter = null, string[] includes = null);
    Task<SubscriptionForViewDto> GetAsync(Expression<Func<Subscription, bool>> filter, string[] includes = null);
    Task<SubscriptionForViewDto> CreateAsync(SubscriptionForCreateDto dto);
    Task<SubscriptionForViewDto> UpdateAsync(Guid id, SubscriptionForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<Subscription, bool>> filter);
    Task<bool> IsSubscriptionActiveAsync(Guid userId);
    Task<SubscriptionForViewDto?> GetActiveSubscriptionAsync(Guid userId);
    Task ExtendSubscriptionAsync(Guid subscriptionId, long additionalDays);
}
