namespace CustomerService.Application.Commands.CreateCustomer;

public class CreateCustomerResult
{
    public int CustomerId { get; init; }
    public string CustomerNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }

    public static CreateCustomerResult SuccessResult(int customerId, string customerNumber, string email, string fullName)
    {
        return new CreateCustomerResult
        {
            CustomerId = customerId,
            CustomerNumber = customerNumber,
            Email = email,
            FullName = fullName,
            Success = true,
            Message = "Customer created successfully",
            CreatedAt = DateTime.UtcNow
        };
    }

    public static CreateCustomerResult FailureResult(string message)
    {
        return new CreateCustomerResult
        {
            Success = false,
            Message = message,
            CreatedAt = DateTime.UtcNow
        };
    }
}