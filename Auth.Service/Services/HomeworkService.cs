using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Homeworks;
using Auth.Service.DTOs.Homeworks.HomeworksDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class HomeworkService : IHomeworkService
{
    private readonly IGenericRepository<Homework> repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper mapper;

    public HomeworkService(IGenericRepository<Homework> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.repository = repository;
        this.mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HomeworkForViewDto> CreateAsync(HomeworkForCreationDto dto)
    {
        if (dto.Image == null || dto.Image.Length == 0)
            throw new ArgumentException("Image is required.");

        // save file
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
        var filePath = Path.Combine("wwwroot/images/homeworks", fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.Image.CopyToAsync(stream);
        }

        // map dto → entity
        var homework = mapper.Map<Homework>(dto);
        homework.ImagePath = $"images/homeworks/{fileName}"; // relative path

        await repository.CreateAsync(homework);
        await repository.SaveChangesAsync();

        // map entity → view dto
        var result = mapper.Map<HomeworkForViewDto>(homework);

        // prepend full URL for client
        result.ImagePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/{homework.ImagePath}";

        return result;
    }




    public async Task<bool> DeleteAsync(Expression<Func<Homework, bool>> filter)
    {
        var homework = await repository.GetAsync(filter);
        if (homework is null)
            return false;

        await repository.DeleteAsync(homework);
        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<HomeworkForViewDto>> GetAllAsync(Expression<Func<Homework, bool>> filter = null, string[] includes = null)
    {
        var homeworks = await repository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<HomeworkForViewDto>>(homeworks);
    }

    public async Task<HomeworkForViewDto> GetAsync(Expression<Func<Homework, bool>> filter, string[] includes = null)
    {
        var homework = await repository.GetAsync(filter, includes);
        return mapper.Map<HomeworkForViewDto>(homework);
    }

    public async Task<HomeworkForViewDto> UpdateAsync(Guid id, HomeworkForUpdateDto dto)
    {
        var homework = await repository.GetAsync(h => h.Id == id);
        if (homework is null)
            throw new HttpStatusCodeException(404, $"Homework with Id {id} not found");

        // Update text fields if provided
        if (!string.IsNullOrEmpty(dto.Definition))
            homework.Definition = dto.Definition;

        if (!string.IsNullOrEmpty(dto.Answer))
            homework.Answer = dto.Answer;

        // Replace image if new file provided
        if (dto.Image != null && dto.Image.Length > 0)
        {
            // Delete old file if exists
            if (!string.IsNullOrEmpty(homework.ImagePath))
            {
                var oldFilePath = Path.Combine("wwwroot", homework.ImagePath);
                if (File.Exists(oldFilePath))
                    File.Delete(oldFilePath);
            }

            // Save new file
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
            var filePath = Path.Combine("wwwroot/images/homeworks", fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.Image.CopyToAsync(stream);

            homework.ImagePath = $"images/homeworks/{fileName}";
        }

        repository.Update(homework);
        await repository.SaveChangesAsync();

        var homeworkWithLesson = await repository.GetAsync(
            h => h.Id == homework.Id,
            new[] { "Lesson" });

        return mapper.Map<HomeworkForViewDto>(homeworkWithLesson);
    }



}
