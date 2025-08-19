using DateHelper = Tahil.Common.Helpers;

namespace Tahil.Domain.Entities;

public class ClassSession : Base
{
    public DateOnly Date { get; set; }
    public int ScheduleId { get; set; }
    public ClassSessionStatus Status { get; set; }
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

    public ICollection<StudentAttendance> StudentAttendances { get; set; } = new List<StudentAttendance>();

    public void Update(ClassSessionDto sessionDto, string userName) 
    {
        RoomId = sessionDto.RoomId;
        TeacherId = sessionDto.TeacherId;
        Date = sessionDto.Date;
        StartTime = sessionDto.StartTime;
        EndTime = sessionDto.EndTime;

        UpdatedBy = userName;
        UpdatedAt = DateHelper.Date.Now;
    }

    public void UpdateStatus(ClassSessionStatus status, string userName) 
    {
        Status = status;

        UpdatedBy = userName;
        UpdatedAt = DateHelper.Date.Now;
    }

    public void UpdateStudentAttendance(List<StudentAttendance> studentAttendances, string userName, Guid tenantId)
    {
        var studentAttendancesToUpdate = StudentAttendances.Where(sg => studentAttendances.Any(s => sg.Id == s.Id)).ToList();
        foreach (var attendance in studentAttendancesToUpdate)
        {
            var attendanceDto = studentAttendances.FirstOrDefault(r => r.Id == attendance.Id)!;
            
            attendance.Update(attendanceDto);
            attendance.UpdatedAt = DateHelper.Date.Now;
            attendance.UpdatedBy = userName;
        }

        var newStudentAttendances = studentAttendances.Where(s => !StudentAttendances.Any(sg => sg.StudentId == s.StudentId)).ToList();
        foreach (var attendance in newStudentAttendances)
        {
            attendance.CreatedBy = userName;
            attendance.UpdatedBy = userName;
            attendance.CreatedAt = DateHelper.Date.Now;
            attendance.UpdatedAt = DateHelper.Date.Now;
            attendance.TenantId = tenantId;

            StudentAttendances.Add(attendance);
        }
    }

}