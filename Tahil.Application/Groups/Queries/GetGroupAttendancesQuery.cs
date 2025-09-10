namespace Tahil.Application.Groups.Queries;

public record GetGroupAttendancesQuery(int Id, int Year, int Month) : IQuery<Result<List<GroupDailyAttendance>>>;

public class GetGroupAttendancesQueryHandler(IGroupRepository groupRepository, IApplicationContext applicationContext) : IQueryHandler<GetGroupAttendancesQuery, Result<List<GroupDailyAttendance>>>
{
    public async Task<Result<List<GroupDailyAttendance>>> Handle(GetGroupAttendancesQuery request, CancellationToken cancellationToken)
    {
        return await groupRepository.GetGroupAttendancesAsync(request.Id, request.Year, request.Month, applicationContext.TenantId);
    }
}