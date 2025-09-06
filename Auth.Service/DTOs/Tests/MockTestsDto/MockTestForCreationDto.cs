using Auth.Domain.Entities.Courses;
using Auth.Domain.Enums;
using Auth.Service.DTOs.Tests.MockTestQuestionsDto;

namespace Auth.Service.DTOs.Tests.MockTestsDto;

public class MockTestForCreationDto
{
    public string Title { get; set; }
    public string Description { get; set; }

    public Guid CourseLevelId { get; set; } // dynamic level
    public TimeSpan Duration { get; set; }
    public DateTime ScheduledAt { get; set; }

}
