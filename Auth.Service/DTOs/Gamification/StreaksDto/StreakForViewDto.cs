namespace Auth.Service.DTOs.Gamification.StreaksDto;

public class StreakForViewDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateOnly LastActivityDate { get; set; }
}
