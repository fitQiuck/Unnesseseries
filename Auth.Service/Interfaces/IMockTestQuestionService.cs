using Auth.Domain.Entities.Tests;
using Auth.Service.DTOs.Tests.MockTestQuestionsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IMockTestQuestionService
{
    Task<IEnumerable<MockTestQuestionForViewDto>> GetAllAsync(Expression<Func<MockTestQuestion, bool>> filter = null, string[] includes = null);
    Task<MockTestQuestionForViewDto> GetAsync(Expression<Func<MockTestQuestion, bool>> filter, string[] includes = null);
    Task<MockTestQuestionForViewDto> CreateAsync(MockTestQuestionForCreationDto dto);
    Task<MockTestQuestionForViewDto> UpdateAsync(Guid id, MockTestQuestionForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<MockTestQuestion, bool>> filter);
}
