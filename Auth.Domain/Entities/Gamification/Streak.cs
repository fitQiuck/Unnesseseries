using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Gamification;

public class Streak : Auditable
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateOnly LastActivityDate { get; set; }

    public ICollection<StreakLog> Logs { get; set; }
}
