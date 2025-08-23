namespace Tahil.Domain.Repositories;

public interface ILookupRepository
{
    Task<Result<List<LookupDto>>> GetGroupsAsync(Guid tenantId);
    Task<Result<List<LookupDto>>> GetRoomsAsync(Guid tenantId);
    Task<Result<List<LookupDto>>> GetCoursesAsync(Guid tenantId);
    Task<Result<List<LookupDto>>> GetTeachersAsync(Guid tenantId);
    Task<Result<List<LookupDto>>> GetStudentsAsync(Guid tenantId);
    Task<ClassScheduleLookupsDto> GetClassScheduleAsync(Guid tenantId);
    Task<ClassSessionLookupsDto> GetClassSessionAsync(Guid tenantId, int courseId, int userId, UserRole userRole);
}