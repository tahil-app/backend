namespace Tahil.Application.StudentAttendancs.Queries;

public record GetStudentMonthlyAttendanceQuery(int StudentId, int Year) : IQuery<Result<List<StudentMonthlyAttendanceDto>>>;

public class GetStudentMonthlyAttendanceQueryHandler(IStudentAttendnceRepository attendnceRepository, IApplicationContext applicationContext) : IQueryHandler<GetStudentMonthlyAttendanceQuery, Result<List<StudentMonthlyAttendanceDto>>>
{

    public async Task<Result<List<StudentMonthlyAttendanceDto>>> Handle(GetStudentMonthlyAttendanceQuery request, CancellationToken cancellationToken)
    {
        return await attendnceRepository.GetStudentMonthlyAttendancesAsync(request.StudentId, request.Year, applicationContext.TenantId);
    }

}
