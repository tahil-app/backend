namespace Tahil.Application.Courses.Queries;

public record GetAllCoursesQuery() : IQuery<Result<List<CourseDto>>>;

public class GetAllCoursesQueryHandler(ICourseRepository courseRepository) : IQueryHandler<GetAllCoursesQuery, Result<List<CourseDto>>>
{
    public async Task<Result<List<CourseDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.GetAllReadOnlyAsync(r => r.IsActive);
        var activeCourses = courses.Adapt<List<CourseDto>>();

        return Result.Success(activeCourses);
    }
}