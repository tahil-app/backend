namespace Tahil.Application.Courses.Queries;

public record GetAllCoursesQuery() : IQuery<Result<List<CourseDto>>>;

public class GetAllCoursesQueryHandler(ICourseRepository courseRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllCoursesQuery, Result<List<CourseDto>>>
{
    public async Task<Result<List<CourseDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.GetAllReadOnlyAsync(r => r.IsActive && r.TenantId == applicationContext.TenantId);
        var activeCourses = courses.Adapt<List<CourseDto>>();

        return Result.Success(activeCourses);
    }
}