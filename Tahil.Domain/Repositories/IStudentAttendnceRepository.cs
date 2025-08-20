namespace Tahil.Domain.Repositories;

public interface IStudentAttendnceRepository: IRepository<StudentAttendance>
{
    Task<Result<StudentAttendanceDisplay>> GetAttendances(int sessionId, Guid tenantId);
    Task<Result<List<StudentMonthlyAttendanceDto>>> GetStudentMonthlyAttendancesAsync(int year, int student, Guid tenantId);
    Task<bool> ExistsInTenantAsync(int id, Guid tenantId);
}