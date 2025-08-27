using CustomerService.Domain.Entities;
using CustomerService.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<Customer> Customers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CustomerConfiguration());

        // Apply all configurations in assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Customer>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Any())
            .SelectMany(x => x.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in ChangeTracker.Entries<Customer>().Select(x => x.Entity))
        {
            entity.ClearDomainEvents();
        }

        // Publish domain events (implement later with MediatR or RabbitMQ)
        foreach (var domainEvent in domainEvents)
        {
            // TODO: Publish to event bus
            // await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Customer>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(Customer.UpdatedDate)).CurrentValue = DateTime.UtcNow;
            }
        }

        return await SaveChangesAsync(cancellationToken);
    }
}