namespace Tahil.Domain.Repositories;

public interface IStudentRepository : IRepository<Student> 
{
    Task<StudentDto> GetStudentAsync(int id);
    Task<string> GetAttachmentDisplayNameAsync(string attachmentName);
    Task<Result<bool>> AddStudentAsync(Student student, Guid tenantId);
    Task<Result<bool>> DeleteStudentAsync(int id);
}