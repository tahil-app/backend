namespace Tahil.Application.Groups.Queries;

public record GetAllGroupsQuery() : IQuery<Result<List<LookupDto>>>;

public class GetAllGroupsQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllGroupsQuery, Result<List<LookupDto>>>
{
    public async Task<Result<List<LookupDto>>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        return await lookupRepository.GetGroupsAsync(applicationContext.TenantId);
    }
}