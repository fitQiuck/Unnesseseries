using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Courses.CourseCommentsDto;
using Auth.Service.DTOs.Courses.UserCoursesDto;
using Auth.Service.Exceptions;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class UserCourseService : IUserCourseService
{
    private readonly IGenericRepository<CourseComment> commentRepository;
    private readonly IGenericRepository<UserCourse> repository;
    private readonly IMapper mapper;

    public UserCourseService(IGenericRepository<UserCourse> repository, IMapper mapper, 
        IGenericRepository<CourseComment> commentRepository)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.commentRepository = commentRepository;
    }

    public async Task<IEnumerable<UserCourseForViewDto>> GetAllAsync(Expression<Func<UserCourse, bool>> filter = null, string[] includes = null)
    {
        var userCourses = repository.GetAll(filter, includes);
        return userCourses.Select(mapper.Map<UserCourseForViewDto>);
    }

    public async Task<UserCourseForViewDto> GetAsync(Expression<Func<UserCourse, bool>> filter, string[] includes = null)
    {
        var userCourse = await repository.GetAsync(filter, includes);
        return mapper.Map<UserCourseForViewDto>(userCourse);
    }

    public async Task<UserCourseForViewDto> CreateAsync(UserCourseForCreationDto dto)
    {
        var entity = mapper.Map<UserCourse>(dto);
        var created = await repository.CreateAsync(entity);
        await repository.SaveChangesAsync(); // Ensure changes are saved asynchronously
        return mapper.Map<UserCourseForViewDto>(created);
    }

    public async ValueTask<UserCourse> UpdateAsync(Guid userId, Guid courseId, UserCourseForUpdateDto dto)
    {
        var entity = await repository.GetAsync(
            uc => uc.UserId == userId && uc.CourseId == courseId
        );

        if (entity is null)
            throw new HttpStatusCodeException(404, $"UserCourse with UserId {userId} and CourseId {courseId} not found");

        entity = mapper.Map(dto, entity);

        entity.UpdatedAt = DateTime.UtcNow;
        repository.Update(entity);
        await repository.SaveChangesAsync();

        return entity;
    }


    public async Task<bool> DeleteAsync(Expression<Func<UserCourse, bool>> filter)
    {
        var level = await repository.GetAsync(filter)
            ?? throw new HttpStatusCodeException(404, "Course not found");

        await repository.DeleteAsync(level);
        await repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddCommentAsync(CourseCommentForCreationDto dto)
    {
        // Check if user bought the course
        var userCourse = await repository.GetAsync(x =>
                x.UserId == dto.UserId && x.CourseId == dto.CourseId);

        if (userCourse == null)
            throw new Exception("Siz bu kursni sotib olmagansiz va comment yoza olmaysiz.");

        var comment = new CourseComment
        {
            UserId = dto.UserId,
            CourseId = dto.CourseId,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = dto.UserId
        };

        await commentRepository.CreateAsync(comment);
        await commentRepository.SaveChangesAsync();

        return true;

    }
}
