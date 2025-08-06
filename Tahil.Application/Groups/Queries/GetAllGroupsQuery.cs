namespace Tahil.Application.Groups.Queries;

public record GetAllGroupsQuery() : IQuery<Result<List<GroupDto>>>;

public class GetAllGroupsQueryHandler(IGroupRepository groupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllGroupsQuery, Result<List<GroupDto>>>
{
    public async Task<Result<List<GroupDto>>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await groupRepository.GetAllReadOnlyAsync(r => r.TenantId == applicationContext.TenantId);

        var activeGroups = groups.Adapt<List<GroupDto>>();

        return Result.Success(activeGroups);
    }
}