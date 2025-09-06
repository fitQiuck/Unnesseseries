namespace Auth.Service.DTOs.SubscriptionPlans;

public class SubscriptionPlanForCreationDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int DurationInDays { get; set; }
}
