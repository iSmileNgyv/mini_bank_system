using CustomerService.Domain.Entities;
using CustomerService.Domain.Interfaces;
using MediatR;

namespace CustomerService.Application.Commands.CreateCustomer;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerDomainService _customerDomainService;

    public CreateCustomerHandler(
        IUnitOfWork unitOfWork, 
        ICustomerDomainService customerDomainService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _customerDomainService = customerDomainService ?? throw new ArgumentNullException(nameof(customerDomainService));
    }

    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // Business validations
            var canCreate = await _customerDomainService.CanCreateCustomerAsync(
                command.Email, 
                string.Empty, // CustomerNumber will be generated
                cancellationToken);

            if (!canCreate)
            {
                return CreateCustomerResult.FailureResult("Customer with this email already exists.");
            }

            // Generate unique customer number
            var customerNumber = await _customerDomainService.GenerateCustomerNumberAsync(
                command.BranchCode ?? "DEFAULT", 
                cancellationToken);

            // Create domain entity
            var customer = Customer.Create(
                customerNumber: customerNumber,
                firstName: command.FirstName,
                lastName: command.LastName,
                email: command.Email,
                phoneNumber: command.PhoneNumber,
                dateOfBirth: command.DateOfBirth,
                street: command.Street,
                city: command.City,
                country: command.Country,
                customerType: command.CustomerType,
                branchCode: command.BranchCode,
                relationshipManagerId: command.RelationshipManagerId);

            // Save to repository
            var savedCustomer = await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return success result
            return CreateCustomerResult.SuccessResult(
                customerId: savedCustomer.Id,
                customerNumber: savedCustomer.CustomerNumber,
                email: savedCustomer.Email.Value,
                fullName: savedCustomer.GetFullName());
        }
        catch (Exception ex)
        {
            // Log exception if needed
            return CreateCustomerResult.FailureResult($"Error creating customer: {ex.Message}");
        }
    }
}