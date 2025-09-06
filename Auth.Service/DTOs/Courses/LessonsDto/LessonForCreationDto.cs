using Microsoft.AspNetCore.Http;

namespace Auth.Service.DTOs.Courses.LessonsDto;

public class LessonForCreationDto
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public Guid CourseId { get; set; }
    public int? Order { get; set; }   // optional; if null we’ll default to 0 or compute

    public IFormFile Video { get; set; } = default!; // required upload
}
