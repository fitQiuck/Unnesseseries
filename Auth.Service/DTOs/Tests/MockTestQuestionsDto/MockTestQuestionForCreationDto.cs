using Auth.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Auth.Service.DTOs.Tests.MockTestQuestionsDto;

public class MockTestQuestionForCreationDto
{
    public TestSection Section { get; set; }       // Listening / Reading
    public QuestionType Format { get; set; }     // MCQ, FillInTheBlank, etc.

    public string? QuestionText { get; set; }
    public string? PassageText { get; set; }        // for reading passages (optional)

    // files come in via multipart/form-data; controller action should be [FromForm]
    public IFormFile? AudioFile { get; set; }       // optional, for listening
    public IFormFile? ImageFile { get; set; }       // optional, for diagrams

    // how you check correctness (string compare, trimmed, case-insensitive)
    // For MCQ you can set "A" / "B" / option text; backend only compares strings
    public string CorrectAnswer { get; set; }
}
