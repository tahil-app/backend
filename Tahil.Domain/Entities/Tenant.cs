namespace Tahil.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Subdomain { get; set; }
    public string? LogoUrl { get; set; } 
    public bool IsActive { get; set; } = true;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<Group> Groups { get; set; } = new List<Group>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    public ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
    public ICollection<ClassSession> Sessions { get; set; } = new List<ClassSession>();
    public ICollection<StudentAttendance> StudentAttendances { get; set; } = new List<StudentAttendance>();

}
