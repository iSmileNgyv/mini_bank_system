namespace CustomerService.Domain.ValueObjects;

using System.Text.RegularExpressions;

public class Email
{
    public string Value { get; private set; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty", nameof(value));

        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email format", nameof(value));

        Value = value.ToLowerInvariant().Trim();
    }

    private static bool IsValidEmail(string email)
    {
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    public string GetDomain() => Value.Split('@')[1];
    
    public override string ToString() => Value;
    
    protected bool Equals(Email other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is Email other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    
    public static implicit operator string(Email email) => email.Value;
}