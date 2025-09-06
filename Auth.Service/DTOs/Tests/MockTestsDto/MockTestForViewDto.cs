using Auth.Domain.Entities.Courses;
using Auth.Domain.Enums;
using Auth.Service.DTOs.Tests.MockTestQuestionsDto;
using Auth.Service.DTOs.Tests.MockTestResultsDto;

namespace Auth.Service.DTOs.Tests.MockTestsDto;

public class MockTestForViewDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public Guid CourseLevelId { get; set; }
    public string CourseLevelName { get; set; }

    public TimeSpan Duration { get; set; }
    public DateTime ScheduledAt { get; set; }

    public IList<MockTestQuestionForViewDto> Questions { get; set; }
}
