using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Subscriptions;
using Auth.Service.DTOs.Subscriptions;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IGenericRepository<Subscription> repository;
    private readonly IMapper mapper;

    public SubscriptionService(IGenericRepository<Subscription> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<SubscriptionForViewDto>> GetAllAsync(Expression<Func<Subscription, bool>> filter = null, string[] includes = null)
    {
        var subscriptions = await repository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<SubscriptionForViewDto>>(subscriptions);
    }

    public async Task<SubscriptionForViewDto> GetAsync(Expression<Func<Subscription, bool>> filter, string[] includes = null)
    {
        var subscription = await repository.GetAsync(
        filter,
        includes: new[] { nameof(Subscription.User), nameof(Subscription.Plan) });

        if (subscription is null)
            throw new HttpStatusCodeException(404, "Subscription not found");

        return mapper.Map<SubscriptionForViewDto>(subscription);
    }

    public async Task<SubscriptionForViewDto> CreateAsync(SubscriptionForCreateDto dto)
    {
        var subscription = mapper.Map<Subscription>(dto);
        await repository.CreateAsync(subscription);
        await repository.SaveChangesAsync();

        return mapper.Map<SubscriptionForViewDto>(subscription);
    }

    public async Task<SubscriptionForViewDto> UpdateAsync(Guid id, SubscriptionForUpdateDto dto)
    {
        var subscription = await repository.GetAsync(s => s.Id == id);
        if (subscription is null)
            throw new HttpStatusCodeException(404, "Subscription not found");

        subscription = mapper.Map(dto, subscription);
        repository.Update(subscription);
        await repository.SaveChangesAsync();

        return mapper.Map<SubscriptionForViewDto>(subscription);
    }

    public async Task<bool> DeleteAsync(Expression<Func<Subscription, bool>> filter)
    {
        var subscription = await repository.GetAsync(filter);
        if (subscription is null)
            throw new HttpStatusCodeException(404, "Subscription not found");

        await repository.DeleteAsync(subscription);
        await repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> IsSubscriptionActiveAsync(Guid userId)
    {
        var now = DateTime.UtcNow;
        var subscription = await repository.GetAsync(s => s.UserId == userId && s.EndDate >= now);
        return subscription != null;
    }

    public async Task<SubscriptionForViewDto?> GetActiveSubscriptionAsync(Guid userId)
    {
        var now = DateTime.UtcNow;
        var sub = await repository.GetAsync(s => s.UserId == userId && s.EndDate >= now, new[] { "User", "Plan" });
        return sub is null ? null : mapper.Map<SubscriptionForViewDto>(sub);
    }

    public async Task ExtendSubscriptionAsync(Guid subscriptionId, long additionalDays)
    {
        var subscription = await repository.GetAsync(s => s.Id == subscriptionId)
            ?? throw new HttpStatusCodeException(404, "Subscription not found");

        subscription.EndDate = subscription.EndDate.AddDays(additionalDays);
        await repository.SaveChangesAsync();
    }
}
