namespace Tahil.Domain.Repositories;

public interface IStudentAttendnceRepository: IRepository<StudentAttendance>
{
    Task<Result<StudentAttendanceDisplay>> GetAttendances(int sessionId, Guid tenantId);
    Task<Result<List<StudentMonthlyAttendanceDto>>> GetStudentMonthlyAttendancesAsync(int student, int year, Guid tenantId);
    Task<Result<List<StudentDailyAttendanceDto>>> GetStudentDailyAttendancesAsync(int student, int year, int month, Guid tenantId);
    Task<bool> ExistsInTenantAsync(int id, Guid tenantId);
}