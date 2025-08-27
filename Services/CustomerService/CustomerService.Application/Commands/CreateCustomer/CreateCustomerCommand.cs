using CustomerService.Domain.Enums;
using MediatR;

namespace CustomerService.Application.Commands.CreateCustomer;

public class CreateCustomerCommand : IRequest<CreateCustomerResult>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string? PostalCode { get; init; }
    public string? State { get; init; }
    public CustomerType CustomerType { get; init; } = CustomerType.Individual;
    public string? BranchCode { get; init; }
    public int? RelationshipManagerId { get; init; }
}