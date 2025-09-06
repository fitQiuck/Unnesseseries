using Auth.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Auth.Service.DTOs.Tests.MockTestQuestionsDto;

public class MockTestQuestionForUpdateDto
{
    public TestSection? Section { get; set; }
    public QuestionType? Format { get; set; }

    public string? QuestionText { get; set; }
    public string? PassageText { get; set; }

    public IFormFile? AudioFile { get; set; }       // replace file if provided
    public IFormFile? ImageFile { get; set; }

    public bool? RemoveAudio { get; set; }         // allow clearing files
    public bool? RemoveImage { get; set; }
}
