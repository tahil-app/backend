namespace Tahil.Domain.Repositories;

public interface ICourseRepository : IRepository<Course> 
{
    Task AddCourseAsync(Course course);
}