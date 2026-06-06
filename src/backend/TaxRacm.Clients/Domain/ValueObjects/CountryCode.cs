namespace TaxRacm.Clients.Domain.ValueObjects;

/// <summary>ISO 3166-1 alpha-2 country code value object.</summary>
public record CountryCode
{
    public string Value { get; }

    public CountryCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 2)
            throw new ArgumentException("Country code must be a 2-letter ISO code.", nameof(value));
        Value = value.ToUpperInvariant();
    }

    public override string ToString() => Value;
}
