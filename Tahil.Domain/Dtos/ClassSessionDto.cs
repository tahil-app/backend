namespace Tahil.Domain.Dtos;

public class ClassSessionDto: BaseDto
{
    public DateOnly Date { get; set; }
    public int ScheduleId { get; set; }
    public ClassSessionStatus Status { get; set; }
    public int TeacherId { get; set; }
    public int RoomId { get; set; }
    public int CourseId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public int? NumberOfStudents { get; set; }
    public string? RoomName { get; set; }
    public string? GroupName { get; set; }
    public string? CourseName { get; set; }
    public string? TeacherName { get; set; }
}