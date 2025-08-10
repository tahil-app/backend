namespace Tahil.Application.ClassSchedules.Queries;

public record GetMonthlyClassSchedulesQuery(int Month, int Year) : IQuery<Result<List<ClassScheduleDto>>>;

public class GetClassSchedulesPagedQueryHandler(IClassScheduleRepository ClassScheduleRepository) : IQueryHandler<GetMonthlyClassSchedulesQuery, Result<List<ClassScheduleDto>>>
{
    public async Task<Result<List<ClassScheduleDto>>> Handle(GetMonthlyClassSchedulesQuery request, CancellationToken cancellationToken)
    {
        var schedules = await ClassScheduleRepository.GetMonthySchedulesAsync(request.Month, request.Year);

        return Result.Success(schedules);
    }
}