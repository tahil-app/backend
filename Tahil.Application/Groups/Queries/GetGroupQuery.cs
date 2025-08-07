namespace Tahil.Application.Groups.Queries;

public record GetGroupQuery(int Id) : IQuery<Result<GroupDto>>;

public class GetGroupQueryHandler(IGroupRepository groupRepository, IApplicationContext applicationContext) : IQueryHandler<GetGroupQuery, Result<GroupDto>>
{
    public async Task<Result<GroupDto>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
    {
        return await groupRepository.GetGroupAsync(request.Id, applicationContext.TenantId);
    }
}