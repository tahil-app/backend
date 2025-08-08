namespace Tahil.Application.Groups.Queries;

public record GetAllGroupsQuery() : IQuery<Result<List<GroupDto>>>;

public class GetAllGroupsQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllGroupsQuery, Result<List<GroupDto>>>
{
    public async Task<Result<List<GroupDto>>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        return await lookupRepository.GetGroupsAsync(applicationContext.TenantId);
    }
}