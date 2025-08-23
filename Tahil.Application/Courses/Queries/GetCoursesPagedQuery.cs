namespace Tahil.Application.Courses.Queries;

public record GetCoursesPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<CourseDto>>>;

public class GetCoursesPagedQueryHandler(ICourseRepository courseRepository, IApplicationContext applicationContext) : IQueryHandler<GetCoursesPagedQuery, Result<PagedList<CourseDto>>>
{
    public async Task<Result<PagedList<CourseDto>>> Handle(GetCoursesPagedQuery request, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.GetPagedProjectionAsync(
            request.QueryParams, 
            s => new CourseDto 
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                IsActive = s.IsActive,
                NumberOfTeachers = s.TeacherCourses.Count,
            },
            r => r.TenantId == applicationContext.TenantId);

        var pagedCourses = courses.Adapt<PagedList<CourseDto>>();
        return Result.Success(pagedCourses);
    }
}