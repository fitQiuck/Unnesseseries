namespace Auth.Service.DTOs.Homeworks.UserHomeworksDto;

public class UserHomeworkForCreationDto
{
    public Guid UserId { get; set; }
    public Guid HomeworkId { get; set; }
    public string Answer { get; set; }
}
