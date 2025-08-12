namespace Tahil.Domain.Entities;

public class ClassSession : Base
{
    public DateOnly Date { get; set; }
    public int ScheduleId { get; set; }
    public LessonSessionStatus Status { get; set; }
    public Guid? TenantId { get; set; }

    public int? OverrideTeacherId { get; set; }
    public int? OverrideRoomId { get; set; }
    public TimeOnly? OverrideStartTime { get; set; }
    public TimeOnly? OverrideEndTime { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = default!;
    public string UpdatedBy { get; set; } = default!;

    public Room? OverrideRoom { get; set; }
    public Teacher? OverrideTeacher { get; set; }
    public Tenant? Tenant { get; set; }
    public ClassSchedule? Schedule { get; set; }
}