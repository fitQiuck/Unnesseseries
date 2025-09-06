using Auth.Domain.Common;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Enums;

namespace Auth.Domain.Entities.Tests;

public class MockTest : Auditable
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string? MaterialPath { get; set; }

    // Foreign key to CourseLevel (since you use entity, not enum)
    public Guid CourseLevelId { get; set; }
    public CourseLevel CourseLevel { get; set; }

    public TimeSpan Duration { get; set; }
    public DateTime ScheduledAt { get; set; }

    // Navigation
    public ICollection<MockTestQuestion> Questions { get; set; }
    public ICollection<TestResult> Results { get; set; }
}
