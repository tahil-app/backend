namespace Tahil.Application.Teachers.Queries;

public record GetTeacherSchedulesQuery(int Id) : IQuery<Result<List<DailyScheduleDto>>>;

public class GetTeacherSchedulesQueryHandler(ITeacherRepository teacherRepository, IApplicationContext applicationContext) : IQueryHandler<GetTeacherSchedulesQuery, Result<List<DailyScheduleDto>>>
{
    public async Task<Result<List<DailyScheduleDto>>> Handle(GetTeacherSchedulesQuery request, CancellationToken cancellationToken)
    {
        var teacherGroups = await teacherRepository.GetTeacherSchedulesAsync(request.Id, applicationContext.TenantId);
        return Result.Success(teacherGroups);
    }
}
