using Auth.Domain.Common;

namespace Auth.Domain.Entities.Gamification;

public class DailyChallengge : Auditable
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int RewardPoints { get; set; }
    public DateTime AvailableDate { get; set; }

    public ICollection<Streak> Streaks { get; set; }
}
