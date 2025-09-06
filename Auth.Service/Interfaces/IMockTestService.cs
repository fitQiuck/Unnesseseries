using Auth.Domain.Entities.Tests;
using Auth.Service.DTOs.Tests.MockTestsDto;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IMockTestService
{
    Task<MockTestForViewDto> CreateAsync(MockTestForCreationDto dto);
    Task<MockTestForViewDto> UpdateAsync(Guid id, MockTestForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<MockTest, bool>> predicate);
    Task<MockTestForViewDto> GetAsync(Expression<Func<MockTest, bool>> predicate, string[] includes = null);
    Task<MockTestForViewDto> AttachMaterialAsync(Guid testId, IFormFile file);
    Task<IEnumerable<MockTestForViewDto>> GetAllAsync(Expression<Func<MockTest, bool>> predicate = null, string[] includes = null);
}
