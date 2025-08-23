namespace Tahil.Application.Students.Queries;

public record GetAllStudentsQuery() : IQuery<Result<List<LookupDto>>>;

public class GetAllStudentsQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllStudentsQuery, Result<List<LookupDto>>>
{
    public async Task<Result<List<LookupDto>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        return await lookupRepository.GetStudentsAsync(applicationContext.TenantId);
    }
}