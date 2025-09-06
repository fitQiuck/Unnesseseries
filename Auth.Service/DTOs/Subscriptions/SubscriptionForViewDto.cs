namespace Auth.Service.DTOs.Subscriptions;

public class SubscriptionForViewDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string UserFullName { get; set; } = string.Empty;

    public Guid PlanId { get; set; }

    public string PlanName { get; set; } = string.Empty;

    public decimal PlanPrice { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }
}
