namespace Tahil.Domain.Entities;

public class Room : Base
{
    public string Name { get; set; } = default!;
    public int Capacity { get; set; }
    public bool IsActive { get; set; }

    public Guid TenantId { get; set; }

    public Tenant? Tenant { get; set; }

    public ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
    public ICollection<ClassSession> Sessions { get; set; } = new List<ClassSession>();

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
