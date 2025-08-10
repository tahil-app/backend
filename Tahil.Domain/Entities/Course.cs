namespace Tahil.Domain.Entities;

public class Course : Base
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public bool IsActive { get; set; }
    public Guid TenantId { get; set; }

    public Tenant? Tenant { get; set; }

    public ICollection<Group> Groups { get; set; } = new List<Group>();
    //public ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
    //public ICollection<ClassSession> Sessions { get; set; } = new List<ClassSession>();
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

    public void UpdateTeachers(List<Teacher> teachers) 
    {
        var newTeachers = teachers.Where(s => !TeacherCourses.Any(sg => sg.TeacherId == s.Id)).ToList();
        var removedTeachers = TeacherCourses.Where(sg => !teachers.Any(s => s.Id == sg.TeacherId)).ToList();

        foreach (var teacher in newTeachers)
        {
            TeacherCourses.Add(new TeacherCourse { TeacherId = teacher.Id, CourseId = Id });
        }

        foreach (var teacher in removedTeachers)
        {
            TeacherCourses.Remove(teacher);
        }
    }
}
