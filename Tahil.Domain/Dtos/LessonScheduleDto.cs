namespace Tahil.Domain.Dtos;

public class LessonScheduleDto: BaseDto
{
    public int RoomId { get; set; }
    public string? RoomName { get; set; }
    
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    
    public int TeacherId { get; set; }
    public string? TeacherName { get; set; }
    
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
    
    public int? ReferenceId { get; set; }

    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LessonScheduleStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = default!;
    public string UpdatedBy { get; set; } = default!;
}