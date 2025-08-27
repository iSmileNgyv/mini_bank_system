using CustomerService.Domain.Events;

namespace CustomerService.Domain.Entities.Common;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
    public virtual DateTime? UpdatedDate { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void SetUpdatedDate()
    {
        UpdatedDate = DateTime.UtcNow;
    }
}