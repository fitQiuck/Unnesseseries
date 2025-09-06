using Auth.Domain.Entities.Subscriptions;
using Auth.Service.DTOs.SubscriptionPlans;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ISubscriptionPlanService
{
    Task<SubscriptionPlanForViewDto> CreateAsync(SubscriptionPlanForCreationDto dto);

    Task<SubscriptionPlanForViewDto> UpdateAsync(Guid id, SubscriptionPlanForUpdateDto dto);

    Task<bool> DeleteAsync(Expression<Func<SubscriptionPlan, bool>> predicate);

    Task<SubscriptionPlanForViewDto> GetAsync(Expression<Func<SubscriptionPlan, bool>> predicate);

    Task<IEnumerable<SubscriptionPlanForViewDto>> GetAllAsync(Expression<Func<SubscriptionPlan, bool>>? predicate = null);
}
