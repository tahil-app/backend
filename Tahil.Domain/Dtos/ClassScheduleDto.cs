namespace Tahil.Domain.Dtos;

public class ClassScheduleDto: BaseDto
{
    public int RoomId { get; set; }
    public int GroupId { get; set; }
    public string? Color { get; set; }

    public WeekDays Day { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ClassScheduleStatus Status { get; set; }

    public string? GroupName { get; set; }
    public string? RoomName { get; set; }
    public string? CourseName { get; set; }
    public string? TeacherName { get; set; }
}