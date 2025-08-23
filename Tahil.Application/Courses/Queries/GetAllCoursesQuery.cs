namespace Tahil.Application.Courses.Queries;

public record GetAllCoursesQuery() : IQuery<Result<List<LookupDto>>>;

public class GetAllCoursesQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllCoursesQuery, Result<List<LookupDto>>>
{
    public async Task<Result<List<LookupDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        return await lookupRepository.GetCoursesAsync(applicationContext.TenantId);
    }
}