namespace Tahil.Domain.Repositories;

public interface IStudentRepository : IRepository<Student> 
{
    Task<StudentDto> GetStudentAsync(int id);
    Task<string> GetAttachmentDisplayNameAsync(string attachmentName);
    Task AddStudentAsync(Student student);
}