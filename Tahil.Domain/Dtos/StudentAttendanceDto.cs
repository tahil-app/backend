namespace Tahil.Domain.Dtos;

public class StudentAttendanceDisplay
{
    public string? RoomName { get; set; }
    public string? GroupName { get; set; }
    public string? CourseName { get; set; }
    public DateOnly? SessionDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public ClassSessionStatus? SessionStatus { get; set; }

    public List<StudentAttendanceDto> Attendances { get; set; } = new();
}

public class StudentAttendanceDto: BaseDto
{
    public int SessionId { get; set; }
    public int StudentId { get; set; }
    public AttendanceStatus? Status { get; set; }
    public string? Note { get; set; }

    public string? StudentName { get; set; }
}