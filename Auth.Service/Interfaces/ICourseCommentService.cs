using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Courses.CourseCommentsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ICourseCommentService
{
    Task<CourseCommentForViewDto> AddCommentAsync(CourseCommentForCreationDto dto);
    Task<CourseCommentForViewDto> GetAsync(Expression<Func<CourseComment, bool>> filter, string[] includes = null);
    Task<CourseCommentForViewDto> UpdateAsync(Guid commentId, CourseCommentForUpdateDto dto);
    Task<bool> DeleteCommentAsync(Expression<Func<CourseComment, bool>> filter);

    // Fix for CS1061: Add missing method signature
    Task<CourseCommentForViewDto> GetCommentByIdAsync(Guid commentId);
}
