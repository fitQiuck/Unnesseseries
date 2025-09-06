using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;
using Auth.Domain.Enums;

namespace Auth.Domain.Entities.Subscriptions;

public class Subscription : Auditable
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid PlanId { get; set; }
    public SubscriptionPlan Plan { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive => EndDate >= DateTime.UtcNow;
}
