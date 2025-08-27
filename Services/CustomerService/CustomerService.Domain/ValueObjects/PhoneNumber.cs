namespace CustomerService.Domain.ValueObjects;

using System.Text.RegularExpressions;
public class PhoneNumber
{
    public string Value { get; private set; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number cannot be empty", nameof(value));
        var cleaned = Regex.Replace(value.Trim(), @"[^\d+]", "");
        
        if (!IsValidPhoneNumber(cleaned))
            throw new ArgumentException("Invalid phone number format", nameof(value));

        Value = cleaned;
    }

    private static bool IsValidPhoneNumber(string phone)
    {
        if (phone.Length < 10 || phone.Length > 15)
            return false;

        var pattern = @"^\+?\d{10,14}$";
        return Regex.IsMatch(phone, pattern);
    }

    public string GetFormattedNumber()
    {
        if (Value.StartsWith("+"))
            return Value;
        
        if (Value.Length == 10)
            return $"({Value.Substring(0, 3)}) {Value.Substring(3, 3)}-{Value.Substring(6, 4)}";
        
        return Value;
    }

    public override string ToString() => Value;
    protected bool Equals(PhoneNumber other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is PhoneNumber other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    
    public static implicit operator string(PhoneNumber phone) => phone.Value;
}