using Auth.Domain.Enums;

namespace Auth.Service.DTOs.Courses.CoursesDto;

public class CourseForUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid? LevelId { get; set; }
}
