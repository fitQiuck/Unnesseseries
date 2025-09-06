namespace Auth.Service.DTOs.Gamification.DailyChallengesDto;

public class DailyChallengeForCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int? RewardPoints { get; set; }
    public DateTime AvailableDate { get; set; }
}
