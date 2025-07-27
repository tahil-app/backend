namespace Tahil.Domain.Consts;

public class Policies
{
    public static string AdminOnly { get; } = "AdminOnly";
    public static string AdminOrEmployee { get; } = "AdminOrEmployee";
    public static string UserOnly { get; } = "UserOnly";
    public static string ALL { get; } = "ALL";
}