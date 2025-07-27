namespace Tahil.Domain.Repositories;

public interface ITeacherRepository : IRepository<Teacher> 
{
    Task AddTeacherAsync(Teacher teacher);
}