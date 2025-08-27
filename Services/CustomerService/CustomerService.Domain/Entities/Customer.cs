using CustomerService.Domain.Entities.Common;

namespace CustomerService.Domain.Entities;

using ValueObjects;
using Events;
using Enums;

public class Customer : BaseEntity
{
    public int Id { get; private set; }
    public string CustomerNumber { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Address Address { get; private set; }
    public CustomerType CustomerType { get; private set; }
    public AccountStatus AccountStatus { get; private set; }
    public KycStatus KycStatus { get; private set; }
    public RiskLevel RiskLevel { get; private set; }
    public string? BranchCode { get; private set; }
    public int? RelationshipManagerId { get; private set; }
    public ProfilePhoto? ProfilePhoto { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Domain Events
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    // Private constructor for EF Core
    private Customer() { }

    // Factory method
    public static Customer Create(
        string customerNumber,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        DateTime dateOfBirth,
        string street,
        string city,
        string country,
        CustomerType customerType = CustomerType.Individual,
        string? branchCode = null,
        int? relationshipManagerId = null)
    {
        if (string.IsNullOrWhiteSpace(customerNumber))
            throw new ArgumentException("Customer number cannot be empty", nameof(customerNumber));
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));
        if (dateOfBirth >= DateTime.UtcNow.AddYears(-18))
            throw new ArgumentException("Customer must be at least 18 years old", nameof(dateOfBirth));

        var customer = new Customer
        {
            CustomerNumber = customerNumber,
            FirstName = firstName,
            LastName = lastName,
            Email = new Email(email),
            PhoneNumber = new PhoneNumber(phoneNumber),
            DateOfBirth = dateOfBirth,
            Address = new Address(street, city, country),
            CustomerType = customerType,
            AccountStatus = AccountStatus.Active,
            KycStatus = KycStatus.Pending,
            RiskLevel = RiskLevel.Low,
            BranchCode = branchCode,
            RelationshipManagerId = relationshipManagerId,
            CreatedAt = DateTime.UtcNow
        };

        customer.AddDomainEvent(new CustomerCreatedEvent(customer.Id, customer.Email.Value, customer.CustomerNumber, customer.GetFullName()));
        return customer;
    }

    public void UpdateProfile(string firstName, string lastName, string phoneNumber, string street, string city, string country)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = new PhoneNumber(phoneNumber);
        Address = new Address(street, city, country);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CustomerProfileUpdatedEvent(Id, Email.Value));
    }

    public void UploadProfilePhoto(string fileName, string filePath, long fileSize, string contentType, int width, int height)
    {
        ProfilePhoto = new ProfilePhoto(fileName, filePath, fileSize, contentType, width, height);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ProfilePhotoUploadedEvent(Id, fileName, fileSize, contentType, width, height, ProfilePhoto.InternalUrl));
    }

    public string GetFullName() => $"{FirstName} {LastName}";
    public int GetAge() => DateTime.UtcNow.Year - DateOfBirth.Year;
    public bool HasProfilePhoto() => ProfilePhoto != null;

    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}