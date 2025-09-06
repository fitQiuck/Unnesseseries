namespace Auth.Service.DTOs.Gamification.StreakLogDto;

public class StreakLogForCreationDto
{
    public Guid StreakId { get; set; }
    public DateOnly ActivityDate { get; set; }
    public string Description { get; set; }
}
