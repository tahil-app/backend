namespace Tahil.Domain.Dtos;

public class StudentDailyAttendanceDto
{
    public DateOnly Date { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Note { get; set; }
    public string? CourseName { get; set; }
}