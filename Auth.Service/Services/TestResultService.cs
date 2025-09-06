using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Tests;
using Auth.Service.DTOs.Tests.MockTestResultsDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace Auth.Service.Services;

public class TestResultService : ITestResultService
{
    private readonly IGenericRepository<TestResult> _repository;
    private readonly IMapper _mapper;
    public TestResultService(IGenericRepository<TestResult> repository, IMapper mapper)
    {
        this._repository = repository;
        this._mapper = mapper;
    }

    public async Task<ResultForViewDto> UpdateAsync(Guid id, ResultForUpdateDto dto)
    {
        var testResult = await _repository.GetAsync(t => t.Id == id)
            ?? throw new HttpStatusCodeException(404, $"TestResult with ID {id} not found");

        testResult = _mapper.Map(dto, testResult);

        _repository.Update(testResult);
        await _repository.SaveChangesAsync();

        return _mapper.Map<ResultForViewDto>(testResult);
    }

    public async Task<bool> DeleteAsync(Expression<Func<TestResult, bool>> filter)
    {
        var testResult = await _repository.GetAsync(filter)
            ?? throw new HttpStatusCodeException(404, "TestResult not found");

        var result = await _repository.DeleteAsync(testResult);
        await _repository.SaveChangesAsync();

        return result;
    }

    public async Task<ResultForViewDto> GetAsync(Expression<Func<TestResult, bool>> filter, string[] includes = null)
    {
        var testResult = await _repository.GetAsync(filter, includes)
            ?? throw new HttpStatusCodeException(404, "TestResult not found");

        return _mapper.Map<ResultForViewDto>(testResult);
    }

    public async Task<IEnumerable<ResultForViewDto>> GetAllAsync(Expression<Func<TestResult, bool>> filter = null, string[] includes = null)
    {
        var testResults = _repository.GetAll(filter, includes);
        return _mapper.Map<IEnumerable<ResultForViewDto>>(testResults);
    }

    // Get results by user
    public async Task<IEnumerable<ResultForViewDto>> GetByUserIdAsync(Guid userId)
    {
        var results = await _repository.GetAll(x => x.UserId == userId, includes: new[] { "MockTest" }).ToListAsync();
        return _mapper.Map<IEnumerable<ResultForViewDto>>(results);
    }

    // Get results by mock test
    public async Task<IEnumerable<ResultForViewDto>> GetByMockTestIdAsync(Guid mockTestId)
    {
        var results = await _repository.GetAll(x => x.MockTestId == mockTestId, includes: new[] { "User" }).ToListAsync();
        return _mapper.Map<IEnumerable<ResultForViewDto>>(results);
    }

    // Calculate average overall score
    public async Task<double> CalculateAverageScoreAsync(Guid userId)
    {
        var results = _repository.GetAll(x => x.UserId == userId);
        if (!results.Any()) return 0;

        var avg = results.Average(r => r.OverallScore);
        return Math.Round(avg, 2);
    }
}
