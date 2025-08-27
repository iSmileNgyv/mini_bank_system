using FluentValidation;

namespace CustomerService.Application.Commands.CreateCustomer;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        // First Name validation
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(2, 100).WithMessage("First name must be between 2 and 100 characters.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("First name can only contain letters and spaces.");

        // Last Name validation  
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 100).WithMessage("Last name must be between 2 and 100 characters.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("Last name can only contain letters and spaces.");

        // Email validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please provide a valid email address.")
            .MaximumLength(320).WithMessage("Email cannot exceed 320 characters.");

        // Phone Number validation
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[\d\s\-\(\)]{10,15}$").WithMessage("Please provide a valid phone number.");

        // Date of Birth validation
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(BeValidAge).WithMessage("Customer must be at least 18 years old.")
            .Must(BeRealisticAge).WithMessage("Please provide a realistic date of birth.");

        // Address validation
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street address is required.")
            .Length(5, 500).WithMessage("Street address must be between 5 and 500 characters.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .Length(2, 100).WithMessage("City must be between 2 and 100 characters.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s\-]+$").WithMessage("City can only contain letters, spaces, and hyphens.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .Length(2, 100).WithMessage("Country must be between 2 and 100 characters.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s\-]+$").WithMessage("Country can only contain letters, spaces, and hyphens.");

        // Optional fields validation
        RuleFor(x => x.PostalCode)
            .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.PostalCode));

        RuleFor(x => x.State)
            .MaximumLength(100).WithMessage("State cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.State));

        // Customer Type validation
        RuleFor(x => x.CustomerType)
            .IsInEnum().WithMessage("Please provide a valid customer type.");

        // Branch Code validation (optional)
        RuleFor(x => x.BranchCode)
            .Length(3, 10).WithMessage("Branch code must be between 3 and 10 characters.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Branch code can only contain uppercase letters and numbers.")
            .When(x => !string.IsNullOrWhiteSpace(x.BranchCode));

        // Relationship Manager validation (optional)
        RuleFor(x => x.RelationshipManagerId)
            .GreaterThan(0).WithMessage("Relationship manager ID must be greater than 0.")
            .When(x => x.RelationshipManagerId.HasValue);
    }

    // Custom validation methods
    private static bool BeValidAge(DateTime dateOfBirth)
    {
        var age = DateTime.UtcNow.Year - dateOfBirth.Year;
        if (DateTime.UtcNow < dateOfBirth.AddYears(age))
            age--;
        
        return age >= 18;
    }

    private static bool BeRealisticAge(DateTime dateOfBirth)
    {
        var age = DateTime.UtcNow.Year - dateOfBirth.Year;
        if (DateTime.UtcNow < dateOfBirth.AddYears(age))
            age--;
            
        return age >= 18 && age <= 120; // Realistic age range
    }
}