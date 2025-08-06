namespace Tahil.Application.Groups.Queries;

public record GetGroupQuery(int Id) : IQuery<Result<GroupDto>>;

public class GetGroupQueryHandler(IGroupRepository groupRepository) : IQueryHandler<GetGroupQuery, Result<GroupDto>>
{
    public async Task<Result<GroupDto>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
    {
        var groupDto = await groupRepository.GetGroupAsync(request.Id);
        return Result.Success(groupDto);
    }
}