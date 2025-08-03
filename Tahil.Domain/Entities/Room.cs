namespace Tahil.Domain.Entities;

public class Room : Base
{
    public string Name { get; set; } = default!;
    public int Capacity { get; set; }
    public bool IsActive { get; set; }

    public Guid TenantId { get; set; }

    public Tenant? Tenant { get; set; }

    public ICollection<LessonSchedule> Schedules { get; set; } = new List<LessonSchedule>();
    public ICollection<LessonSession> Sessions { get; set; } = new List<LessonSession>();

    public void Validate()
    {
        Check.IsNull(Name, nameof(Name));
        Check.IsNull(TenantId, nameof(TenantId));
    }

    public void Update(RoomDto roomDto) 
    {
        Name = roomDto.Name;
        Capacity = roomDto.Capacity;

        Validate();
    }

    public void Activate() => IsActive = true;
    public void DeActivate() => IsActive = false;
}
