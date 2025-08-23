namespace Tahil.Domain.Dtos;

public class StudentDailyAttendanceDto
{
    public DateOnly Date { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Note { get; set; }
}