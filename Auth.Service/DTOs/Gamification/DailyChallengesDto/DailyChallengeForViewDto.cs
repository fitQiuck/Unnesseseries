namespace Auth.Service.DTOs.Gamification.DailyChallengesDto;

public class DailyChallengeForViewDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime AvailableDate { get; set; }
}
