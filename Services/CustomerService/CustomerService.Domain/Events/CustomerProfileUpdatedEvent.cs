namespace CustomerService.Domain.Events;

public class CustomerProfileUpdatedEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public int CustomerId { get; }
    public string Email { get; }
    public Dictionary<string, object> Changes { get; }

    public CustomerProfileUpdatedEvent(int customerId, string email, Dictionary<string, object> changes)
    {
        CustomerId = customerId;
        Email = email;
        Changes = changes ?? new Dictionary<string, object>();
    }

    public CustomerProfileUpdatedEvent(int customerId, string email) 
        : this(customerId, email, new Dictionary<string, object>())
    {
    }
}