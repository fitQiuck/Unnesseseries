namespace Auth.Service.DTOs.Homeworks.HomeworksDto;

public class HomeworkForViewDto
{
    public Guid Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public Guid LessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
}
