using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Service.DTOs.Courses.CourseCommentsDto;
using Auth.Service.DTOs.Courses.CourseLevelsDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class CourseLevelService : ICourseLevelService
{
    private readonly IGenericRepository<CourseLevel> courseLevelRepository;
    private readonly IMapper mapper;

    public CourseLevelService(IGenericRepository<CourseLevel> repository, IMapper mapper)
    {
        courseLevelRepository = repository;
        this.mapper = mapper;
    }

    public async Task<CourseLevelForViewDto> CreateAsync(CourseLevelForCreationDto dto)
    {
        var exists = await courseLevelRepository.GetAsync(cl => cl.Name == dto.Name);
        if (exists is not null)
            throw new AlreadyExistsException("Course level already exists");

        var level = mapper.Map<CourseLevel>(dto);
        await courseLevelRepository.CreateAsync(level);
        await courseLevelRepository.SaveChangesAsync();

        return mapper.Map<CourseLevelForViewDto>(level);
    }

    public async Task<bool> DeleteAsync(Expression<Func<CourseLevel, bool>> filter)
    {
        var level = await courseLevelRepository.GetAsync(filter)
            ?? throw new HttpStatusCodeException(404, "Course level not found");

        await courseLevelRepository.DeleteAsync(level);
        await courseLevelRepository.SaveChangesAsync();

        return true;
    }

    public async Task<CourseLevelForViewDto> GetAsync(Expression<Func<CourseLevel, bool>> filter, string[] includes = null)
    {
        var courseLevel = await courseLevelRepository.GetAsync(filter, includes);
        if (courseLevel is null)
            throw new HttpStatusCodeException(404, "CommentLevel not found");

        return mapper.Map<CourseLevelForViewDto>(courseLevel);
    }

    public async Task<IEnumerable<CourseLevelForViewDto>> GetAllAsync(Expression<Func<CourseLevel, bool>> filter = null, string[] includes = null)
    {
        var levels = await courseLevelRepository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<CourseLevelForViewDto>>(levels);
    }

    public async Task<CourseLevelForViewDto> UpdateAsync(Guid id, CourseLevelForUpdateDto dto)
    {
        var level = await courseLevelRepository.GetAsync(l => l.Id == id)
        ?? throw new HttpStatusCodeException(404, "Course level not found");

        level = mapper.Map(dto, level);

        courseLevelRepository.Update(level);
        await courseLevelRepository.SaveChangesAsync();

        return mapper.Map<CourseLevelForViewDto>(level);
    }
}
