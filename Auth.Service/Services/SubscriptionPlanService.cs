using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Subscriptions;
using Auth.Service.DTOs.SubscriptionPlans;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class SubscriptionPlanService : ISubscriptionPlanService
{
    private readonly IGenericRepository<SubscriptionPlan> repository;
    private readonly IMapper mapper;

    public SubscriptionPlanService(IGenericRepository<SubscriptionPlan> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }


    public async Task<SubscriptionPlanForViewDto> CreateAsync(SubscriptionPlanForCreationDto dto)
    {
        var plan = mapper.Map<SubscriptionPlan>(dto);
        await repository.CreateAsync(plan);
        await repository.SaveChangesAsync();
        return mapper.Map<SubscriptionPlanForViewDto>(plan);
    }

    public async Task<SubscriptionPlanForViewDto> UpdateAsync(Guid id, SubscriptionPlanForUpdateDto dto)
    {
        var plan = await repository.GetAsync(p => p.Id == id)
            ?? throw new HttpStatusCodeException(404, "Subscription plan not found");

        plan = mapper.Map(dto, plan);

        repository.Update(plan);
        await repository.SaveChangesAsync();
        return mapper.Map<SubscriptionPlanForViewDto>(plan);
    }

    public async Task<bool> DeleteAsync(Expression<Func<SubscriptionPlan, bool>> predicate)
    {
        var subscription = await repository.GetAsync(predicate);
        if (subscription is null)
            throw new HttpStatusCodeException(404, "Subscription not found");

        await repository.DeleteAsync(subscription);
        await repository.SaveChangesAsync();

        return true;
    }

    public async Task<SubscriptionPlanForViewDto> GetAsync(Expression<Func<SubscriptionPlan, bool>> predicate)
    {
        var plan = await repository.GetAsync(predicate)
            ?? throw new HttpStatusCodeException(404, "Subscription plan not found");

        return mapper.Map<SubscriptionPlanForViewDto>(plan);
    }

    public async Task<IEnumerable<SubscriptionPlanForViewDto>> GetAllAsync(Expression<Func<SubscriptionPlan, bool>>? predicate = null)
    {
        var plans = await repository.GetAll(predicate).ToListAsync();
        return mapper.Map<IEnumerable<SubscriptionPlanForViewDto>>(plans);
    }
}
