using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Gamification;
using Auth.Service.DTOs.Courses.CourseLevelsDto;
using Auth.Service.DTOs.Courses.CoursesDto;
using Auth.Service.Exceptions;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class CourseService : ICourseService
{
    private readonly IGenericRepository<Course> courseRepository;
    private readonly IGenericRepository<CourseLevel> courseLevelRepository;
    private readonly IMapper mapper;

    public CourseService(IGenericRepository<Course> repository,
        IMapper mapper, IGenericRepository<CourseLevel> repository1)
    {
        this.mapper = mapper;
        this.courseRepository = repository;
        this.courseLevelRepository = repository1;
    }

    public async Task<IEnumerable<CourseForViewDto>> GetAllAsync(
        Expression<Func<Course, bool>> filter = null,
        string[] includes = null)
    {
        var levels = await courseRepository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<CourseForViewDto>>(levels);
    }

    public async Task<CourseForViewDto> GetAsync(Expression<Func<Course, bool>> filter, string[] includes = null)
    {
        var courseLevel = await courseRepository.GetAsync(filter, includes);
        if (courseLevel is null)
            throw new HttpStatusCodeException(404, "Course not found");

        return mapper.Map<CourseForViewDto>(courseLevel);
    }

    public async Task<CourseForViewDto> CreateAsync(CourseForCreationDto dto)
    {
        var existingCourse = await courseRepository.GetAsync(c => c.Title == dto.Title);
        if (existingCourse is not null)
            throw new HttpStatusCodeException(409, "Course with this title already exists");

        var course = mapper.Map<Course>(dto);
        await courseRepository.CreateAsync(course);
        await courseRepository.SaveChangesAsync();

        // ✅ Re-fetch with navigation properties
        var createdCourse = await courseRepository.GetAsync(
            c => c.Id == course.Id,
            includes: new[] { "Level", "Comments" }
        );

        var result = mapper.Map<CourseForViewDto>(createdCourse);
        return result;
    }

    public async Task<CourseForViewDto> UpdateAsync(Guid id, CourseForUpdateDto dto)
    {
        var course = await courseRepository.GetAsync(c => c.Id == id);
        if (course is null)
            throw new HttpStatusCodeException(404, "Course not found");

        // Only update provided fields
        if (!string.IsNullOrWhiteSpace(dto.Title))
            course.Title = dto.Title;

        if (!string.IsNullOrWhiteSpace(dto.Description))
            course.Description = dto.Description;

        if (dto.LevelId.HasValue)
        {
            var levelExists = await courseLevelRepository.GetAsync(l => l.Id == dto.LevelId.Value);
            if (levelExists is null)
                throw new ArgumentException("Invalid LevelId");

            course.LevelId = dto.LevelId.Value;
        }

        course.UpdatedAt = DateTime.UtcNow;

        courseRepository.Update(course);
        await courseRepository.SaveChangesAsync();

        return mapper.Map<CourseForViewDto>(course);
    }

    public async Task<bool> DeleteAsync(Expression<Func<Course, bool>> filter)
    {
        var level = await courseRepository.GetAsync(filter)
            ?? throw new HttpStatusCodeException(404, "Course not found");

        await courseRepository.DeleteAsync(level);
        await courseRepository.SaveChangesAsync();

        return true;
    }

}
