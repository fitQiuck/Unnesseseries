using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Gamification;
using Auth.Service.DTOs.Gamification.StreaksDto;
using Auth.Service.Interfaces;
using AutoMapper;

namespace Auth.Service.Services;


public class StreakService : IStreakService
{
    private readonly IGenericRepository<Streak> repository;
    private readonly IMapper mapper;

    public StreakService(IGenericRepository<Streak> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<StreakForViewDto> GetByUserIdAsync(Guid userId)
    {
        var streak = await repository.GetAsync(s => s.UserId == userId);
        return mapper.Map<StreakForViewDto>(streak);
    }

    public async Task<StreakForViewDto> CreateAsync(StreakForCreationDto dto)
    {
        var entity = mapper.Map<Streak>(dto);
        var created = await repository.CreateAsync(entity);
        await repository.SaveChangesAsync();

        return mapper.Map<StreakForViewDto>(created);
    }

    public async Task<StreakForViewDto> UpdateAsync(Guid id, StreakForUpdateDto dto)
    {
        var entity = await repository.GetAsync(s => s.Id == id);
        if (entity is null) return null;

        entity = mapper.Map(dto, entity);

        repository.Update(entity);
        await repository.SaveChangesAsync();

        return mapper.Map<StreakForViewDto>(entity);
    }
}
