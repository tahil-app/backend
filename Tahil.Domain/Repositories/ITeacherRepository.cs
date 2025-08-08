namespace Tahil.Domain.Repositories;

public interface ITeacherRepository : IRepository<Teacher> 
{
    Task<TeacherDto> GetTeacherAsync(int id, Guid tenantId);
    Task<string> GetAttachmentDisplayNameAsync(string attachmentName, Guid tenantId);
    Task<Result<bool>> AddTeacherAsync(Teacher teacher, Guid tenantId);
    Task<Result<bool>> UpdateTeacherAsync(TeacherDto teacherDto, Guid tenantId);
    Task<Result<bool>> DeleteTeacherAsync(int id, Guid tenantId);
    Task<bool> ExistsInTenantAsync(int? id, Guid? tenantId);
}