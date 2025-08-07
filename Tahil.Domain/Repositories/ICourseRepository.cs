namespace Tahil.Domain.Repositories;

public interface ICourseRepository : IRepository<Course> 
{
    Task<Result<bool>> AddCourseAsync(Course course, Guid tenantId);
    Task<Result<bool>> DeleteCourseAsync(int id, Guid tenantId);
    Task<Result<CourseDto>> GetCourseAsync(int id, Guid tenantId);
    Task<Result<bool>> UpdateTeachersAsync(int id, List<int> teacherIds, Guid tenantId);
}