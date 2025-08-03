namespace Tahil.Domain.Repositories;

public interface ITeacherRepository : IRepository<Teacher> 
{
    Task<TeacherDto> GetTeacherAsync(int id);
    Task<string> GetAttachmentDisplayNameAsync(string attachmentName);
    Task<Result<bool>> AddTeacherAsync(Teacher teacher, Guid tenantId);
    Task<Result<bool>> DeleteTeacherAsync(int id);
}