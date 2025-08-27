namespace CustomerService.Domain.Events;

public class CustomerCreatedEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public int CustomerId { get; }
    public string Email { get; }
    public string CustomerNumber { get; }
    public string FullName { get; }

    public CustomerCreatedEvent(int customerId, string email, string customerNumber, string fullName)
    {
        CustomerId = customerId;
        Email = email;
        CustomerNumber = customerNumber;
        FullName = fullName;
    }
}