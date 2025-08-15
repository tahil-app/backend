namespace Tahil.Domain.Repositories;

public interface ILookupRepository
{
    Task<Result<List<GroupDto>>> GetGroupsAsync(Guid tenantId);
    Task<Result<List<RoomDto>>> GetRoomsAsync(Guid tenantId);
    Task<Result<List<CourseDto>>> GetCoursesAsync(Guid tenantId);
    Task<Result<List<TeacherDto>>> GetTeachersAsync(Guid tenantId);
    Task<Result<List<StudentDto>>> GetStudentsAsync(Guid tenantId);
    Task<ClassScheduleLookupsDto> GetClassScheduleAsync(Guid tenantId);
    Task<ClassSessionLookupsDto> GetClassSessionAsync(Guid tenantId, int courseId, int userId, UserRole userRole);
}