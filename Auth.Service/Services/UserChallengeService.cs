using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Gamification;
using Auth.Domain.Entities.Homeworks;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Courses.LessonsDto;
using Auth.Service.DTOs.Tests.MockTestResultsDto;
using Auth.Service.DTOs.UserChallenges;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class UserChallengeService : IUserChallengeService
{
    private readonly IGenericRepository<UserChallenge> userChallengeRepository;
    private readonly IUserService userService;
    private readonly IGenericRepository<DailyChallengge> dailyChallengeRepository;
    private readonly IMapper mapper;

    public UserChallengeService(IGenericRepository<UserChallenge> userChallengeRepository,
        IUserService userService,
        IGenericRepository<DailyChallengge> dailyChallengeRepository,
        IMapper mapper)
    {
        this.userChallengeRepository = userChallengeRepository;
        this.userService = userService;
        this.dailyChallengeRepository = dailyChallengeRepository;
        this.mapper = mapper;
    }

    public async Task<UserChallengeForViewDto> CompleteChallengeAsync(UserChallengeForCreationDto dto)
    {
        // Check if already completed
        var existing = await userChallengeRepository.GetAsync(uc =>
            uc.UserId == dto.UserId && uc.ChallengeId == dto.ChallengeId);
        if (existing is not null)
            throw new AlreadyExistsException("Challenge already completed by this user.");

        // Check challenge existence
        var challenge = await dailyChallengeRepository.GetAsync(c => c.Id == dto.ChallengeId)
            ?? throw new HttpStatusCodeException(404, "Challenge not found.");

        // Create entry
        var userChallenge = new UserChallenge
        {
            UserId = dto.UserId,
            ChallengeId = dto.ChallengeId,
            CompletedAt = DateTime.UtcNow
        };

        await userChallengeRepository.CreateAsync(userChallenge);
        await userChallengeRepository.SaveChangesAsync();

        // Add reward points
        //await userService.AddPointsAsync(dto.UserId, challenge.RewardPoints);

        // Fetch with includes to return full view DTO
        var result = await userChallengeRepository.GetAsync(
            uc => uc.Id == userChallenge.Id,
            new[] { nameof(UserChallenge.User), nameof(UserChallenge.Challenge) });

        return mapper.Map<UserChallengeForViewDto>(result);
    }

    public async Task<IEnumerable<UserChallengeForViewDto>> GetAllAsync(Expression<Func<UserChallenge, bool>> filter, string[] includes = null)
    {
        var testResults = userChallengeRepository.GetAll(filter, includes).ToList();
        return mapper.Map<IEnumerable<UserChallengeForViewDto>>(testResults);
    }

    public async Task<UserChallengeForViewDto> GetByIdAsync(Expression<Func<UserChallenge, bool>> filter, string[] includes = null)
    {
        var lesson = await userChallengeRepository.GetAsync(filter, includes)
            ?? throw new HttpStatusCodeException(404, "UserChallenge not found");

        return mapper.Map<UserChallengeForViewDto>(lesson);
    }
}
