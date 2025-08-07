namespace Tahil.Application.Courses.Queries;

public record GetCourseQuery(int Id) : IQuery<Result<CourseDto>>;

public class GetCourseQueryHandler(ICourseRepository courseRepository, IApplicationContext applicationContext) : IQueryHandler<GetCourseQuery, Result<CourseDto>>
{
    public async Task<Result<CourseDto>> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        return await courseRepository.GetCourseAsync(request.Id, applicationContext.TenantId);
    }
}