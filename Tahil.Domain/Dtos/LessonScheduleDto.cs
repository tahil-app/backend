namespace Tahil.Domain.Dtos;

public class LessonScheduleDto: BaseDto
{
    public int RoomId { get; set; }
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public int GroupId { get; set; }
    public int? ReferenceId { get; set; }

    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public LessonScheduleStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = default!;
    public string UpdatedBy { get; set; } = default!;
}