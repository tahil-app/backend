namespace Tahil.Application.Groups.Queries;

public record GetGroupsPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<GroupDto>>>;

public class GetGroupsPagedQueryHandler(IGroupRepository groupRepository) : IQueryHandler<GetGroupsPagedQuery, Result<PagedList<GroupDto>>>
{
    public async Task<Result<PagedList<GroupDto>>> Handle(GetGroupsPagedQuery request, CancellationToken cancellationToken)
    {
        var groups = await groupRepository.GetPagedAsync(request.QueryParams);
        var pagedGroups = groups.Adapt<PagedList<GroupDto>>();

        return Result.Success(pagedGroups);
    }
}