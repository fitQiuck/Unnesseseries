namespace Auth.Service.DTOs.Courses.LessonsDto;

public class LessonForViewDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;

    public int Order { get; set; }

    public string VideoUrl { get; set; } = string.Empty; // full URL for clients
    public int? VideoDurationSeconds { get; set; }
}
