using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Courses.CoursesDto;
using Auth.Service.DTOs.Courses.LessonsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ILessonService
{
    Task<IEnumerable<LessonForViewDto>> GetAllAsync(Expression<Func<Lesson, bool>> filter = null, string[] includes = null);
    Task<LessonForViewDto> GetAsync(Expression<Func<Lesson, bool>> filter, string[] includes = null);
    Task<LessonForViewDto> CreateAsync(LessonForCreationDto dto);
    Task<bool> DeleteAsync(Expression<Func<Lesson, bool>> filter);
    Task<LessonForViewDto> UpdateAsync(Guid id, LessonForUpdateDto dto);
}
