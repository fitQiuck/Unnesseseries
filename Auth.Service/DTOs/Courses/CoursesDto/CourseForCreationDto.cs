using Auth.Domain.Enums;

namespace Auth.Service.DTOs.Courses.CoursesDto;

public class CourseForCreationDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid LevelId { get; set; }
}
