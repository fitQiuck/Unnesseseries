namespace Auth.Service.DTOs.Gamification.StreakLogDto;

public class StreakLogForViewDto
{
    public Guid Id { get; set; }
    public Guid StreakId { get; set; }
    public DateOnly ActivityDate { get; set; }
    public string Description { get; set; }
}
