namespace Auth.Service.DTOs.Homeworks.UserHomeworksDto;

public class UserHomeworkForViewDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid HomeworkId { get; set; }
    public string HomeworkDefinition { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public Guid Score { get; set; }
    public bool IsCompleted { get; set; }
}
