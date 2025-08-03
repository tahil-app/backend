namespace Tahil.Domain.Consts;

public class Policies
{
    public static string AdminOnly { get; } = "AdminOnly";
    public static string AdminOrEmployee { get; } = "AdminOrEmployee";
    public static string TeacherOnly { get; } = "TeacherOnly";
    public static string ALL { get; } = "ALL";
}