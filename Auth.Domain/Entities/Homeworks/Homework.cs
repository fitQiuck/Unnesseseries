using Auth.Domain.Common;
using Auth.Domain.Entities.Courses;
using Microsoft.AspNetCore.Http;

namespace Auth.Domain.Entities.Homeworks;

public class Homework : Auditable
{
    public string ImagePath { get; set; }
    public string Definition { get; set; } = string.Empty;
    public string Answer { get; set; }
    public long CorrectAnswers { get; set; }
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
}
