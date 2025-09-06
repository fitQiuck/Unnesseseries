using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Tests;
using Auth.Service.DTOs.Tests.MockTestsDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class MockTestService : IMockTestService
{
    private readonly IGenericRepository<MockTest> repository;
    private readonly IFileService fileService;
    private readonly IMapper mapper;

    public MockTestService(IGenericRepository<MockTest> repository, IMapper mapper, IFileService fileService)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.fileService = fileService;
    }

    public async Task<MockTestForViewDto> CreateAsync(MockTestForCreationDto dto)
    {
        var entity = mapper.Map<MockTest>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;

        var created = await repository.CreateAsync(entity);
        return mapper.Map<MockTestForViewDto>(created);
    }

    public async Task<MockTestForViewDto> UpdateAsync(Guid id, MockTestForUpdateDto dto)
    {
        var entity = await repository.GetAsync(t => t.Id == id);
        if (entity is null)
            throw new HttpStatusCodeException(404, $"MockTest with Id {id} not found.");

        mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        var updated = repository.Update(entity);
        return mapper.Map<MockTestForViewDto>(updated);
    }

    public async Task<bool> DeleteAsync(Expression<Func<MockTest, bool>> predicate)
    {
        var entity = await repository.GetAsync(predicate);
        if (entity is null)
            throw new HttpStatusCodeException(404, "MockTest not found.");

        return await repository.DeleteAsync(entity);
    }

    public async Task<MockTestForViewDto> GetAsync(Expression<Func<MockTest, bool>> predicate, string[] includes = null)
    {
        var entity = await repository.GetAsync(predicate, includes);
        if (entity is null)
            throw new HttpStatusCodeException(404, "MockTest not found.");

        return mapper.Map<MockTestForViewDto>(entity);
    }

    public async Task<IEnumerable<MockTestForViewDto>> GetAllAsync(Expression<Func<MockTest, bool>> predicate = null, string[] includes = null)
    {
        var entities = await repository.GetAll(predicate, includes).ToListAsync();
        return mapper.Map<IEnumerable<MockTestForViewDto>>(entities);
    }

    public async Task<MockTestForViewDto> AttachMaterialAsync(Guid testId, IFormFile file)
    {
        var test = await repository.GetAsync(t => t.Id == testId);
        if (test is null)
            throw new HttpStatusCodeException(404, "MockTest not found");

        var filePath = await fileService.UploadAsync(file, "TestMaterials");
        test.MaterialPath = filePath;  // property in MockTest entity
        test.UpdatedAt = DateTime.UtcNow;

        var updated = repository.Update(test);
        return mapper.Map<MockTestForViewDto>(updated);
    }
}
