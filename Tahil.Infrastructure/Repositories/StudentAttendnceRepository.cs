using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;

namespace Tahil.Infrastructure.Repositories;

public class StudentAttendnceRepository : Repository<StudentAttendance>, IStudentAttendnceRepository
{
    private readonly BEContext _context;
    public StudentAttendnceRepository(BEContext bEContext) : base(bEContext.Set<StudentAttendance>())
    {
        _context = bEContext;
    }

    public async Task<Result<StudentAttendanceDisplay>> GetAttendances(int sessionId, Guid tenantId)
    {
        // Get the session with basic information
        var session = await _context.Set<ClassSession>()
            .Where(r => r.Id == sessionId && r.TenantId == tenantId)
            .Select(r => new
            {
                r.Date,
                r.StartTime,
                r.EndTime,
                r.Status,
                RoomName = r.Room!.Name,
                GroupName = r.Schedule!.Group!.Name,
                CourseName = r.Schedule!.Group!.Course!.Name,
                GroupId = r.Schedule!.Group!.Id
            })
            .FirstOrDefaultAsync();

        if (session == null)
            return Result<StudentAttendanceDisplay>.Success(new StudentAttendanceDisplay());

        // Get all attendances for this session
        var existingAttendances = await _context.Set<StudentAttendance>()
            .Where(sa => sa.SessionId == sessionId)
            .Select(sa => new StudentAttendanceDto
            {
                Id = sa.Id,
                StudentId = sa.StudentId,
                Status = sa.Status,
                Note = sa.Note,
                StudentName = sa.Student!.User!.Name,
                SessionId = sa.SessionId
            })
            .ToListAsync();

        // First time
        if (!existingAttendances.Any())
        {
            var groupStudents = await _context.Set<StudentGroup>()
                .Where(sg => sg.GroupId == session.GroupId)
                .Select(sg => new
                {
                    sg.StudentId,
                    StudentName = sg.Student!.User!.Name
                })
                .ToListAsync();

            groupStudents.ForEach(stud =>
            {
                existingAttendances.Add(new StudentAttendanceDto
                {
                    SessionId = sessionId,
                    StudentId = stud.StudentId,
                    StudentName = stud.StudentName,
                    Status = AttendanceStatus.None
                });
            });
        }


        // Create attendance display result
        var attendanceResult = new StudentAttendanceDisplay
        {
            RoomName = session.RoomName,
            GroupName = session.GroupName,
            CourseName = session.CourseName,
            SessionDate = session.Date,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            SessionStatus = session.Status,
            Attendances = existingAttendances.OrderBy(r => r.StudentName).ToList()
        };

        return Result<StudentAttendanceDisplay>.Success(attendanceResult);
    }

    public async Task<Result<List<StudentMonthlyAttendanceDto>>> GetStudentMonthlyAttendancesAsync(int year, int student, Guid tenantId)
    {
        var attendances = await _dbSet.Where(r => r.Session!.Date.Year == year && r.StudentId == student && r.TenantId == tenantId)
            .GroupBy(r => r.Session!.Date.Month)
            .Select(r => new StudentMonthlyAttendanceDto
            {
                Present = r.Where(r => r.Status == AttendanceStatus.Present).Count(),
                Absent = r.Where(r => r.Status == AttendanceStatus.Absent).Count(),
                Late = r.Where(r => r.Status == AttendanceStatus.Late).Count(),
                Month = r.Key
            }).ToListAsync();

        if (attendances != null)
        {
            return Result<List<StudentMonthlyAttendanceDto>>.Success(attendances);
        }

        return Result<List<StudentMonthlyAttendanceDto>>.Success(new List<StudentMonthlyAttendanceDto>());
    }

    public async Task<bool> ExistsInTenantAsync(int id, Guid tenantId)
    {
        return await _dbSet.AnyAsync(c => c.SessionId == id && c.TenantId == tenantId);
    }

}