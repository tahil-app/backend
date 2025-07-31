using System.Text.RegularExpressions;

namespace Tahil.Domain.ValueObjects;

public class Email
{
    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        Check.IsNull(email, "Email");
        Check.IsValidEmail(email);

        return new(email.ToLowerInvariant());
    }

    private static bool IsValidEmail(string email)
    {
        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return regex.IsMatch(email);
    }

    public override string ToString() => Value;
}