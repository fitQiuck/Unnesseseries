using Auth.Domain.Enums;

namespace Auth.Service.DTOs.Subscriptions;

public class SubscriptionForCreateDto
{
    public Guid UserId { get; set; }
    public Guid PlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
