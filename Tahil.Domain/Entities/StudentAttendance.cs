
namespace Tahil.Domain.Entities;

public class StudentAttendance: Base
{
    public int SessionId { get; set; }
    public int StudentId { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Note { get; set; }
    public Guid? TenantId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = default!;
    public string UpdatedBy { get; set; } = default!;

    public ClassSession? Session { get; set; }
    public Student? Student { get; set; }
    public Tenant? Tenant { get; set; }


    public void Update(StudentAttendance attendance) 
    {
        SessionId = attendance.SessionId;
        StudentId = attendance.StudentId;
        Status = attendance.Status;
        Note = attendance.Note;
    }
}