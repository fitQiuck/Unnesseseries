using Auth.Domain.Common;
using Auth.Domain.Entities.Gamification;

namespace Auth.Domain.Entities.UserManagement;

public class UserChallenge : Auditable
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid ChallengeId { get; set; }
    public DailyChallengge Challenge { get; set; } = default!;

    public DateTime CompletedAt { get; set; }
}
