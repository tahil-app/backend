namespace Tahil.Domain.Dtos;

public class DailyScheduleDto
{
    public WeekDays Day { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? CourseName { get; set; }
    public string? TeacherName { get; set; }
    public string? RoomName { get; set; }
    public string? GroupName { get; set; }
}