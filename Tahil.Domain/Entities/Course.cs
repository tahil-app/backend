namespace Tahil.Domain.Entities;

public class Course : Base
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsActive { get; set; }
    public Guid TenantId { get; set; }

    public Tenant? Tenant { get; set; }

    public ICollection<Group> Groups { get; set; } = new List<Group>();
    public ICollection<LessonSchedule> Schedules { get; set; } = new List<LessonSchedule>();
    public ICollection<LessonSession> Sessions { get; set; } = new List<LessonSession>();
    public ICollection<TeacherCourse> TeacherCourses { get; set; } = new List<TeacherCourse>();
    public void Validate()
    {
        Check.IsNull(Name, nameof(Name));
        Check.IsNull(TenantId, nameof(TenantId));
    }

    public void Update(CourseDto courseDto) 
    {
        Name = courseDto.Name;
        Description = courseDto.Description;

        Validate();
    }

    public void Activate() => IsActive = true;
    public void DeActivate() => IsActive = false;
}
