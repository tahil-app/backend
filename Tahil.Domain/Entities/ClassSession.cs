namespace Tahil.Domain.Entities;

public class ClassSession : Base
{
    public int RoomId { get; set; }
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public int GroupId { get; set; }
    public int ScheduleId { get; set; }

    public DateOnly Date { get; set; }
    public string Note { get; set; } = default!;
    public LessonSessionStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = default!;
    public string UpdatedBy { get; set; } = default!;

    public Room? Room { get; set; }
    public Course? Course { get; set; }
    public Teacher? Teacher { get; set; }
    public Group? Group { get; set; }
    //public ClassSchedule? Schedule { get; set; }
}