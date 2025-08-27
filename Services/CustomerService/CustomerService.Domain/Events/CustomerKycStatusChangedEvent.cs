using CustomerService.Domain.Enums;

namespace CustomerService.Domain.Events;

public class CustomerKycStatusChangedEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public int CustomerId { get; }
    public KycStatus OldStatus { get; }
    public KycStatus NewStatus { get; }
    public string? Reason { get; }

    public CustomerKycStatusChangedEvent(int customerId, KycStatus oldStatus, KycStatus newStatus, string? reason = null)
    {
        CustomerId = customerId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        Reason = reason;
    }
}