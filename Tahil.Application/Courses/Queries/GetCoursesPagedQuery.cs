namespace Tahil.Application.Courses.Queries;

public record GetCoursesPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<CourseDto>>>;

public class GetCoursesPagedQueryHandler(ICourseRepository courseRepository, IApplicationContext applicationContext) : IQueryHandler<GetCoursesPagedQuery, Result<PagedList<CourseDto>>>
{
    public async Task<Result<PagedList<CourseDto>>> Handle(GetCoursesPagedQuery request, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.GetPagedAsync(request.QueryParams, r => r.TenantId == applicationContext.TenantId, [r => r.TeacherCourses]);
        var pagedCourses = courses.Adapt<PagedList<CourseDto>>();

        return Result.Success(pagedCourses);
    }
}