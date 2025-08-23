namespace Tahil.Application.Teachers.Queries;

public record GetAllTeachersQuery() : IQuery<Result<List<LookupDto>>>;

public class GetAllTeachersQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllTeachersQuery, Result<List<LookupDto>>>
{
    public async Task<Result<List<LookupDto>>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
    {
        return await lookupRepository.GetTeachersAsync(applicationContext.TenantId);
    }
}