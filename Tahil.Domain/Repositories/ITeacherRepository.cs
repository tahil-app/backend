namespace Tahil.Domain.Repositories;

public interface ITeacherRepository : IRepository<Teacher> 
{
    Task<TeacherDto> GetTeacherAsync(int id);
    Task<string> GetAttachmentDisplayNameAsync(string attachmentName);
    Task AddTeacherAsync(Teacher teacher);
}