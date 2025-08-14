namespace Tahil.Domain.Entities;

public class ClassSession : Base
{
    public DateOnly Date { get; set; }
    public int ScheduleId { get; set; }
    public LessonSessionStatus Status { get; set; }
    public Guid? TenantId { get; set; }

    public int? TeacherId { get; set; }
    public int? RoomId { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = default!;
    public string UpdatedBy { get; set; } = default!;

    public Room? Room { get; set; }
    public Teacher? Teacher { get; set; }
    public Tenant? Tenant { get; set; }
    public ClassSchedule? Schedule { get; set; }
}