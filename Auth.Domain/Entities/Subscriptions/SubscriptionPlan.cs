using Auth.Domain.Common;

namespace Auth.Domain.Entities.Subscriptions;

public class SubscriptionPlan : Auditable
{
    public string Name { get; set; } = string.Empty; // e.g. BeginnerOnly, FullCourse

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int DurationInDays { get; set; }

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
