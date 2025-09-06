using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Gamification;
using Auth.Service.DTOs.Courses.CourseLevelsDto;
using Auth.Service.DTOs.Courses.LessonsDto;
using Auth.Service.DTOs.Gamification.DailyChallengesDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class LessonService : ILessonService
{
    private readonly IGenericRepository<Lesson> repository;
    private readonly IMapper mapper;

    public LessonService(IGenericRepository<Lesson> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<LessonForViewDto>> GetAllAsync(Expression<Func<Lesson, bool>> filter = null, string[] includes = null)
    {
        var lessons = await repository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<LessonForViewDto>>(lessons);
    }

    public async Task<LessonForViewDto> GetAsync(Expression<Func<Lesson, bool>> filter, string[] includes = null)
    {
        var lesson = await repository.GetAsync(filter, includes);

        return mapper.Map<LessonForViewDto>(lesson);
    }

    public async Task<LessonForViewDto> CreateAsync(LessonForCreationDto dto)
    {
        var existingLesson = await repository.GetAsync(c => c.Title == dto.Title);
        if (existingLesson is not null)
            throw new HttpStatusCodeException(409, "Lesson with this title already exists");

        var lesson = mapper.Map<Lesson>(dto);

        // Handle video upload
        if (dto.Video != null && dto.Video.Length > 0)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Video.FileName)}";
            var filePath = Path.Combine("wwwroot/videos/lessons", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.Video.CopyToAsync(stream);

            lesson.VideoPath = $"videos/lessons/{fileName}";
        }

        await repository.CreateAsync(lesson);
        await repository.SaveChangesAsync();

        return mapper.Map<LessonForViewDto>(lesson);
    }

    public async Task<bool> DeleteAsync(Expression<Func<Lesson, bool>> filter)
    {
        var lesson = await repository.GetAsync(filter)
            ?? throw new HttpStatusCodeException(404, "Lesson not found");

        // Delete video if exists
        if (!string.IsNullOrEmpty(lesson.VideoPath))
        {
            var oldFilePath = Path.Combine("wwwroot", lesson.VideoPath);
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);
        }

        await repository.DeleteAsync(lesson);
        await repository.SaveChangesAsync();

        return true;
    }

    public async Task<LessonForViewDto> UpdateAsync(Guid id, LessonForUpdateDto dto)
    {
        var lesson = await repository.GetAsync(l => l.Id == id)
            ?? throw new HttpStatusCodeException(404, "Lesson not found");

        // Update text fields if provided
        if (!string.IsNullOrEmpty(dto.Title))
            lesson.Title = dto.Title;

        if (!string.IsNullOrEmpty(dto.Description))
            lesson.Description = dto.Description;

        // Replace video if new file provided
        if (dto.Video != null && dto.Video.Length > 0)
        {
            // Delete old video
            if (!string.IsNullOrEmpty(lesson.VideoPath))
            {
                var oldFilePath = Path.Combine("wwwroot", lesson.VideoPath);
                if (File.Exists(oldFilePath))
                    File.Delete(oldFilePath);
            }

            // Save new file
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Video.FileName)}";
            var filePath = Path.Combine("wwwroot/videos/lessons", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.Video.CopyToAsync(stream);

            lesson.VideoPath = $"videos/lessons/{fileName}";
        }

        repository.Update(lesson);
        await repository.SaveChangesAsync();

        return mapper.Map<LessonForViewDto>(lesson);
    }
}
