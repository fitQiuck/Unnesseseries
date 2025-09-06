using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Homeworks;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Homeworks.UserHomeworksDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class UserHomeworkService : IUserHomeworkService
{
    private readonly IGenericRepository<User> userRepository;
    private readonly IGenericRepository<Homework> homeworkRepository;
    private readonly IGenericRepository<UserHomework> repository;
    private readonly IUserService userService;
    private readonly IMapper mapper;

    public UserHomeworkService(IGenericRepository<UserHomework> repository, IMapper mapper, 
        IGenericRepository<User> userRepository,
        IUserService userService,
        IGenericRepository<Homework> genericRepository)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.userRepository = userRepository;
        this.userService = userService;
        this.homeworkRepository = genericRepository;
    }

    public async Task<IEnumerable<UserHomeworkForViewDto>> GetAllAsync(Expression<Func<UserHomework, bool>> filter = null, string[] includes = null)
    {
        var userHomeworks = repository.GetAll(filter, includes);
        return mapper.Map<IEnumerable<UserHomeworkForViewDto>>(userHomeworks);
    }

    public async Task<UserHomeworkForViewDto> GetAsync(Expression<Func<UserHomework, bool>> filter, string[] includes = null)
    {
        var userHomework = await repository.GetAsync(filter, includes);
        if (userHomework is null)
            throw new HttpStatusCodeException(404, "UserHomework not found");

        return mapper.Map<UserHomeworkForViewDto>(userHomework);
    }

    public async Task<UserHomeworkForViewDto> CreateAsync(UserHomeworkForCreationDto dto)
    {
        var userHomework = mapper.Map<UserHomework>(dto);
        await repository.CreateAsync(userHomework);
        await repository.SaveChangesAsync();

        return mapper.Map<UserHomeworkForViewDto>(userHomework);
    }

    public async Task<UserHomeworkForViewDto> UpdateAsync(Guid id, UserHomeworkForUpdateDto dto)
    {
        var userHomework = await repository.GetAsync(x => x.Id == id);
        if (userHomework is null)
            throw new HttpStatusCodeException(404, "UserHomework not found");

        mapper.Map(dto, userHomework);
        repository.Update(userHomework);
        await repository.SaveChangesAsync();

        return mapper.Map<UserHomeworkForViewDto>(userHomework);
    }

    public async Task<bool> DeleteAsync(Expression<Func<UserHomework, bool>> filter)
    {
        var userHomework = await repository.GetAsync(filter);
        if (userHomework is null)
            return false;

        await repository.DeleteAsync(userHomework);
        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<UserHomeworkForViewDto> CompleteHomeworkAsync(UserHomeworkForCreationDto dto)
    {
        var homework = await homeworkRepository.GetAsync(h => h.Id == dto.HomeworkId)
            ?? throw new HttpStatusCodeException(404, "Homework not found");

        var user = await userRepository.GetAsync(u => u.Id == dto.UserId)
            ?? throw new HttpStatusCodeException(404, "User not found");

        // Check if already completed
        var existing = await repository.GetAsync(uh =>
            uh.UserId == dto.UserId && uh.HomeworkId == dto.HomeworkId);

        if (existing is not null)
            throw new AlreadyExistsException("This homework is already completed by the user");

        var userHomework = new UserHomework
        {
            UserId = dto.UserId,
            HomeworkId = dto.HomeworkId,
            CreatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(userHomework);

        // ✅ Add 1 point for completing homework
        await userService.AddPointsAsync(dto.UserId, 1);

        await repository.SaveChangesAsync();

        return mapper.Map<UserHomeworkForViewDto>(userHomework);
    }

}
