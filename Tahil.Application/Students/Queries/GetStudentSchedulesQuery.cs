namespace Tahil.Application.Students.Queries;

public record GetStudentSchedulesQuery(int Id) : IQuery<Result<List<DailyScheduleDto>>>;

public class GetStudentSchedulesQueryHandler(IStudentRepository studentRepository, IApplicationContext applicationContext) : IQueryHandler<GetStudentSchedulesQuery, Result<List<DailyScheduleDto>>>
{
    public async Task<Result<List<DailyScheduleDto>>> Handle(GetStudentSchedulesQuery request, CancellationToken cancellationToken)
    {
        var studentGroups = await studentRepository.GetStudentSchedulesAsync(request.Id, applicationContext.TenantId);
        return Result.Success(studentGroups);
    }
}
