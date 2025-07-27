namespace Tahil.Application.Courses.Queries;

public record GetCoursesPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<CourseDto>>>;

public class GetCoursesPagedQueryHandler(ICourseRepository courseRepository) : IQueryHandler<GetCoursesPagedQuery, Result<PagedList<CourseDto>>>
{
    public async Task<Result<PagedList<CourseDto>>> Handle(GetCoursesPagedQuery request, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.GetPagedAsync(request.QueryParams);
        var pagedCourses = courses.Adapt<PagedList<CourseDto>>();

        return Result<PagedList<CourseDto>>.Success(pagedCourses);
    }
}