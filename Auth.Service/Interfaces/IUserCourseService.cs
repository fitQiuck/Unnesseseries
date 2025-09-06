using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Gamification;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Courses.CourseCommentsDto;
using Auth.Service.DTOs.Courses.UserCoursesDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IUserCourseService
{
    Task<IEnumerable<UserCourseForViewDto>> GetAllAsync(Expression<Func<UserCourse, bool>> filter = null, string[] includes = null);
    Task<UserCourseForViewDto> GetAsync(Expression<Func<UserCourse, bool>> filter, string[] includes = null);

    Task<UserCourseForViewDto> CreateAsync(UserCourseForCreationDto dto);
    ValueTask<UserCourse> UpdateAsync(Guid id, Guid courseId, UserCourseForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<UserCourse, bool>> filter);

    Task<bool> AddCommentAsync(CourseCommentForCreationDto dto);
}
