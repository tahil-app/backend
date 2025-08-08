namespace Tahil.Application.Courses.Queries;

public record GetAllCoursesQuery() : IQuery<Result<List<CourseDto>>>;

public class GetAllCoursesQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllCoursesQuery, Result<List<CourseDto>>>
{
    public async Task<Result<List<CourseDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        return await lookupRepository.GetCoursesAsync(applicationContext.TenantId);
    }
}