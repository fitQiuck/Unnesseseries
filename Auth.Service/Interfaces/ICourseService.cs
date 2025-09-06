using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Gamification;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Courses.CoursesDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseForViewDto>> GetAllAsync(Expression<Func<Course, bool>> filter = null, string[] includes = null);
    Task<CourseForViewDto> GetAsync(Expression<Func<Course, bool>> filter, string[] includes = null);
    Task<CourseForViewDto> CreateAsync(CourseForCreationDto dto);
    Task<bool> DeleteAsync(Expression<Func<Course, bool>> filter);
    Task<CourseForViewDto> UpdateAsync(Guid id, CourseForUpdateDto dto);

}
