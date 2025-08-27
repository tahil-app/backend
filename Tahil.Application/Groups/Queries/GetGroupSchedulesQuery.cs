namespace Tahil.Application.Groups.Queries;

public record GetGroupSchedulesQuery(int Id) : IQuery<Result<List<DailyScheduleDto>>>;

public class GetGroupSchedulesQueryHandler(IGroupRepository groupRepository, IApplicationContext applicationContext) : IQueryHandler<GetGroupSchedulesQuery, Result<List<DailyScheduleDto>>>
{
    public async Task<Result<List<DailyScheduleDto>>> Handle(GetGroupSchedulesQuery request, CancellationToken cancellationToken)
    {
        var groupGroups = await groupRepository.GetGroupSchedulesAsync(request.Id, applicationContext.TenantId);
        return Result.Success(groupGroups);
    }
}
