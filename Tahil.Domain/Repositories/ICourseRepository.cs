namespace Tahil.Domain.Repositories;

public interface ICourseRepository : IRepository<Course> 
{
    Task<Result<bool>> AddCourseAsync(Course course, Guid tenantId);
    Task<Result<bool>> DeleteCourseAsync(int id);
}