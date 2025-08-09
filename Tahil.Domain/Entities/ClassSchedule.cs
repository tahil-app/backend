using Tahil.Common.Helpers;

namespace Tahil.Domain.Entities;

public class ClassSchedule : Base
{
    public int RoomId { get; set; }
    public int GroupId { get; set; }
    public WeekDays Day { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public ClassScheduleStatus Status { get; set; }
    public Guid? TenantId { get; set; }

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = default!;
    public string UpdatedBy { get; set; } = default!;

    public Room? Room { get; set; }
    public Group? Group { get; set; }
    public Tenant? Tenant { get; set; }

    public ICollection<ClassSession> Sessions { get; set; } = new List<ClassSession>();
    public ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();

    public void Update(ClassScheduleDto scheduleDto, string userName)
    {
        RoomId = scheduleDto.RoomId;
        GroupId = scheduleDto.GroupId;
        
        Day = scheduleDto.Day;
        StartTime = scheduleDto.StartTime;
        EndTime = scheduleDto.EndTime;
        
        StartDate = scheduleDto.StartDate;
        EndDate = scheduleDto.EndDate;
        
        UpdatedAt = Date.Now;
        UpdatedBy = userName;
    }
}