namespace CustomerService.Domain.ValueObjects;

public class Address
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public string? PostalCode { get; private set; }
    public string? State { get; private set; }

    public Address(string street, string city, string country, string? postalCode = null, string? state = null)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty", nameof(street));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty", nameof(country));

        Street = street.Trim();
        City = city.Trim();
        Country = country.Trim();
        PostalCode = postalCode?.Trim();
        State = state?.Trim();
    }

    public string GetFullAddress()
    {
        var parts = new List<string> { Street, City };
        
        if (!string.IsNullOrWhiteSpace(State))
            parts.Add(State);
            
        if (!string.IsNullOrWhiteSpace(PostalCode))
            parts.Add(PostalCode);
            
        parts.Add(Country);
        
        return string.Join(", ", parts);
    }

    public override string ToString() => GetFullAddress();
    
    protected bool Equals(Address other) => 
        Street == other.Street && 
        City == other.City && 
        Country == other.Country && 
        PostalCode == other.PostalCode && 
        State == other.State;
        
    public override bool Equals(object? obj) => obj is Address other && Equals(other);
    
    public override int GetHashCode() => HashCode.Combine(Street, City, Country, PostalCode, State);
}