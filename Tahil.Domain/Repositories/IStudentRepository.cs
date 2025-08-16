namespace Tahil.Domain.Repositories;

public interface IStudentRepository : IRepository<Student> 
{
    Task<StudentDto> GetStudentAsync(int id, Guid tenantId);
    Task<string> GetAttachmentDisplayNameAsync(string attachmentName, Guid tenantId);
    Task<Result<bool>> AddStudentAsync(Student student, Guid tenantId);
    Task<Result<bool>> DeleteStudentAsync(int id, Guid tenantId);
    Task<bool> ExistsInTenantAsync(int id, Guid tenantId);
}