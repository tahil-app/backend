using System.Numerics;
using System.Text.RegularExpressions;
using Tahil.Common.Exceptions;
using Tahil.Domain.Localization;

namespace Tahil.Domain.Helpers;

public static class Check
{
    private static LocalizedStrings? _strings;
    public static void Configure(LocalizedStrings strings)
    {
        _strings = strings;
    }

    public static void IsNull<T>(T entity, string? name = default) where T : notnull
    {
        if (entity == null)
            throw new DomainException($"{name ?? nameof(T)}: {_strings?.CannotBeNull ?? "cannot be null"}");
    }

    public static void IsValidId(int id, string name)
    {
        if (id < 0)
            throw new DomainException($"{name}: {_strings?.MustBePositive ?? "must be greater than zero"}");
    }

    public static void IsPositive<T>(T value, string paramName) where T : struct, INumber<T>
    {
        if (value < T.Zero)
            throw new DomainException($"{paramName}: {_strings?.MustBePositive ?? "must be positive"}");
    }

    public static void IsValidEmail(string email)
    {
        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        bool isValid = regex.IsMatch(email);
        if (!isValid)
            throw new DomainException($"{email}: {_strings?.InvalidEmailFormat ?? " in invalid format"}");
    }
}