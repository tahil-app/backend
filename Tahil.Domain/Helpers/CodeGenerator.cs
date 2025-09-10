namespace Tahil.Domain.Helpers;

public static class CodeGenerator
{
    public static string GetStudentCode(this int code)
    {
        return $"S_{code.ToString("00000")}";
    }

    public static string GetTeacherCode(this int code)
    {
        return $"T_{code.ToString("00000")}";
    }
}