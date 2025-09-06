using Auth.Domain.Enums;

namespace Auth.Service.DTOs.Tests.MockTestQuestionsDto;

public class MockTestQuestionForViewDto
{
    public Guid Id { get; set; }
    public TestSection Section { get; set; }
    public QuestionType Format { get; set; }

    public string QuestionText { get; set; }
    public string PassageText { get; set; }

    public string AudioUrl { get; set; }
    public string ImageUrl { get; set; }

    public string CorrectAnswer { get; set; }
}
