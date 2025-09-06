using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Tests;
using Auth.Service.DTOs.Tests.MockTestQuestionsDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class MockTestQuestionService : IMockTestQuestionService
{
    private readonly IGenericRepository<MockTestQuestion> questionRepository;
    private readonly IMapper mapper;
    private readonly IFileService fileService;

    public MockTestQuestionService(IGenericRepository<MockTestQuestion> questionRepository,
        IMapper mapper,
        IFileService fileService)
    {
        this.questionRepository = questionRepository;
        this.mapper = mapper;
        this.fileService = fileService;
    }

    public async Task<IEnumerable<MockTestQuestionForViewDto>> GetAllAsync(
        Expression<Func<MockTestQuestion, bool>> filter = null,
        string[] includes = null)
    {
        var questions =  questionRepository.GetAll(filter, includes).ToList();
        return mapper.Map<IEnumerable<MockTestQuestionForViewDto>>(questions);
    }

    public async Task<MockTestQuestionForViewDto> GetAsync(
        Expression<Func<MockTestQuestion, bool>> filter,
        string[] includes = null)
    {
        var question = await questionRepository.GetAsync(filter, includes);
        if (question is null)
            throw new HttpStatusCodeException(404, "Question not found");

        return mapper.Map<MockTestQuestionForViewDto>(question);
    }

    public async Task<MockTestQuestionForViewDto> CreateAsync(MockTestQuestionForCreationDto dto)
    {
        // map dto → entity
        var question = mapper.Map<MockTestQuestion>(dto);

        // Handle file uploads
        if (dto.AudioFile != null)
            question.AudioUrl = await fileService.UploadAsync(dto.AudioFile, "mocktests/audio");

        if (dto.ImageFile != null)
            question.ImageUrl = await fileService.UploadAsync(dto.ImageFile, "mocktests/images");

        await questionRepository.CreateAsync(question);
        await questionRepository.SaveChangesAsync();

        return mapper.Map<MockTestQuestionForViewDto>(question);
    }

    public async Task<MockTestQuestionForViewDto> UpdateAsync(Guid id, MockTestQuestionForUpdateDto dto)
    {
        var question = await questionRepository.GetAsync(q => q.Id == id);
        if (question is null)
            throw new HttpStatusCodeException(404, "Question not found");

        // Update only provided fields
        if (dto.Section.HasValue)
            question.TestSection = dto.Section.Value;

        if (dto.Format.HasValue)
            question.QuestionType = dto.Format.Value;

        if (!string.IsNullOrWhiteSpace(dto.QuestionText))
            question.QuestionText = dto.QuestionText;

        if (!string.IsNullOrWhiteSpace(dto.PassageText))
            question.PassageText = dto.PassageText;

        // File handling
        if (dto.AudioFile != null)
            question.AudioUrl = await fileService.UploadAsync(dto.AudioFile, "mocktests/audio");
        else if (dto.RemoveAudio == true)
            question.AudioUrl = null;

        if (dto.ImageFile != null)
            question.ImageUrl = await fileService.UploadAsync(dto.ImageFile, "mocktests/images");
        else if (dto.RemoveImage == true)
            question.ImageUrl = null;

        question.UpdatedAt = DateTime.UtcNow;

        questionRepository.Update(question);
        await questionRepository.SaveChangesAsync();

        return mapper.Map<MockTestQuestionForViewDto>(question);
    }

    public async Task<bool> DeleteAsync(Expression<Func<MockTestQuestion, bool>> filter)
    {
        var question = await questionRepository.GetAsync(filter)
            ?? throw new HttpStatusCodeException(404, "Question not found");

        await questionRepository.DeleteAsync(question);
        await questionRepository.SaveChangesAsync();

        return true;
    }


}
