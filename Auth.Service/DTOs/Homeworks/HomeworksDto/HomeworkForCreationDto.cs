using Microsoft.AspNetCore.Http;

namespace Auth.Service.DTOs.Homeworks.HomeworksDto;

public class HomeworkForCreationDto
{
    public IFormFile? Image { get; set; }       // Uploaded image
    public string Definition { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public Guid LessonId { get; set; }
}
