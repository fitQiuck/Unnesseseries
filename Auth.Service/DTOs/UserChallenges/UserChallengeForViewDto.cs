namespace Auth.Service.DTOs.UserChallenges;

public class UserChallengeForViewDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;

    public Guid ChallengeId { get; set; }
    public string ChallengeTitle { get; set; } = string.Empty;

    public DateTime CompletedAt { get; set; }
}
