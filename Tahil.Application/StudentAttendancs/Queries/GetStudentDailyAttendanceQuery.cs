namespace Tahil.Application.StudentAttendancs.Queries;

public record GetStudentDailyAttendanceQuery(int StudentId, int Year, int Month) : IQuery<Result<List<StudentDailyAttendanceDto>>>;

public class GetStudentDailyAttendanceQueryHandler(IStudentAttendnceRepository attendnceRepository, IApplicationContext applicationContext) : IQueryHandler<GetStudentDailyAttendanceQuery, Result<List<StudentDailyAttendanceDto>>>
{

    public async Task<Result<List<StudentDailyAttendanceDto>>> Handle(GetStudentDailyAttendanceQuery request, CancellationToken cancellationToken)
    {
        return await attendnceRepository.GetStudentDailyAttendancesAsync(request.StudentId, request.Year, request.Month, applicationContext.TenantId);
    }

}
