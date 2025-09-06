using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Courses.CourseCommentsDto;
using Auth.Service.DTOs.Tests.MockTestsDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace Auth.Service.Services;

public class CourseCommentService : ICourseCommentService
{
    private readonly IGenericRepository<CourseComment> repository;
    private readonly IMapper mapper;

    public CourseCommentService(IGenericRepository<CourseComment> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<CourseCommentForViewDto> AddCommentAsync(CourseCommentForCreationDto dto)
    {
        var comment = mapper.Map<CourseComment>(dto);
        comment.CreatedAt = DateTime.UtcNow;

        await repository.CreateAsync(comment);
        await repository.SaveChangesAsync();

        var resultDto = mapper.Map<CourseCommentForViewDto>(comment);
        return resultDto;
    }

    public async Task<CourseCommentForViewDto> GetAsync(Expression<Func<CourseComment, bool>> filter, string[] includes = null)
    {
        var courseComment = await repository.GetAsync(filter, includes);
        if (courseComment is null)
            throw new HttpStatusCodeException(404, "Comment not found");

        return mapper.Map<CourseCommentForViewDto>(courseComment);
    }

    public async Task<CourseCommentForViewDto> UpdateAsync(Guid commentId, CourseCommentForUpdateDto dto)
    {
        var comment = await repository.GetAsync(c => c.Id == commentId)
        ?? throw new HttpStatusCodeException(404, "Comment not found");

        comment = mapper.Map(dto, comment);

        repository.Update(comment);
        await repository.SaveChangesAsync();

        return mapper.Map<CourseCommentForViewDto>(comment);
    }

    public async Task<bool> DeleteCommentAsync(Expression<Func<CourseComment, bool>> filter)
    {
        var comment = await repository.GetAsync(filter)
            ?? throw new HttpStatusCodeException(404, "Comment not found");

        await repository.DeleteAsync(comment);
        await repository.SaveChangesAsync();

        return true;
    }

    public async Task<CourseCommentForViewDto> GetCommentByIdAsync(Guid commentId)
    {
        var comment = await repository.GetAsync(c => c.Id == commentId, new[] { "User" });

        if (comment is null)
            throw new HttpStatusCodeException(404, "Comment not found");

        return mapper.Map<CourseCommentForViewDto>(comment);
    }

}
