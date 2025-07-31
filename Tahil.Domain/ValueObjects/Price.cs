namespace Tahil.Domain.ValueObjects;

public class Price
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Price(decimal amount, string currency)
    {
        Check.IsPositive(amount, "Price");
        Check.IsNull(amount, "Price");

        Amount = amount;
        Currency = currency.ToUpper(); // Normalize currency to uppercase
    }

    public static Price Create(decimal amount, string currency)
    {
        return new(amount, currency);
    }

    public override string ToString()
    {
        return $"{Currency} {Amount:0.00}";
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Price)obj;
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }
}
