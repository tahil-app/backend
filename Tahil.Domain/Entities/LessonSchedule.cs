namespace Tahil.Domain.Entities;

public class LessonSchedule : Base
{
    public int RoomId { get; set; }
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public int GroupId { get; set; }
    
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

    public Room? Room { get; set; }
    public Course? Course { get; set; }
    public Teacher? Teacher { get; set; }
    public Group? Group { get; set; }

    public ICollection<LessonSession> Sessions { get; set; } = new List<LessonSession>();
}