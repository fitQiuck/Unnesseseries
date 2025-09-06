using Microsoft.AspNetCore.Http;

namespace Auth.Service.DTOs.Courses.LessonsDto;

public class LessonForUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Order { get; set; }
    public IFormFile? Video { get; set; }
}
