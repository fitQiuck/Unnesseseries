namespace Auth.Service.DTOs.Gamification.StreaksDto;

public class StreakForUpdateDto
{
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateOnly LastActivityDate { get; set; }
}
