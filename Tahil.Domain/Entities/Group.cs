namespace Tahil.Domain.Entities;

public class Group : Base
{
    public string Name { get; set; } = default!;
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public Guid TenantId { get; set; }
    public int Capacity { get; set; }

    public Course? Course { get; set; }
    public Teacher? Teacher { get; set; }
    public Tenant? Tenant { get; set; }

    public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
    public ICollection<LessonSchedule> Schedules { get; set; } = new List<LessonSchedule>();
    public ICollection<LessonSession> Sessions { get; set; } = new List<LessonSession>();

    public void Validate()
    {
        Check.IsNull(Name, nameof(Name));
        Check.IsNull(TenantId, nameof(TenantId));
        Check.IsValidId(CourseId, nameof(CourseId));
        Check.IsValidId(TeacherId, nameof(TeacherId));
    }

    public void Update(GroupDto groupDto)
    {
        Name = groupDto.Name;
        CourseId = groupDto.CourseId;
        TeacherId = groupDto.TeacherId;
        Capacity = groupDto.Capacity;

        Validate();
    }
}
