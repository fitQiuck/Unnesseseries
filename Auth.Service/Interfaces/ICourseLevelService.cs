using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Courses.CourseLevelsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ICourseLevelService
{
    Task<IEnumerable<CourseLevelForViewDto>> GetAllAsync(Expression<Func<CourseLevel, bool>> filter = null, string[] includes = null);
    Task<CourseLevelForViewDto> GetAsync(Expression<Func<CourseLevel, bool>> filter, string[] includes = null);
    Task<CourseLevelForViewDto> CreateAsync(CourseLevelForCreationDto dto);
    Task<CourseLevelForViewDto> UpdateAsync(Guid id, CourseLevelForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<CourseLevel, bool>> filter);
}
