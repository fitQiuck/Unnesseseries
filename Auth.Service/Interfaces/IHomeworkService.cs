using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Homeworks;
using Auth.Service.DTOs.Homeworks.HomeworksDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IHomeworkService
{
    Task<IEnumerable<HomeworkForViewDto>> GetAllAsync(Expression<Func<Homework, bool>> filter = null, string[] includes = null);
    Task<HomeworkForViewDto> GetAsync(Expression<Func<Homework, bool>> filter, string[] includes = null);
    Task<HomeworkForViewDto> CreateAsync(HomeworkForCreationDto dto);
    Task<HomeworkForViewDto> UpdateAsync(Guid id, HomeworkForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<Homework, bool>> filter);
}
