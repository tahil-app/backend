namespace Tahil.Application.StudentAttendancs.Queries;

public record GetStudentAttendancesQuery(int SessionId) : IQuery<Result<StudentAttendanceDisplay>>;

public class GetStudentAttendancesQueryHandler(IStudentAttendnceRepository attendnceRepository, IApplicationContext applicationContext) : IQueryHandler<GetStudentAttendancesQuery, Result<StudentAttendanceDisplay>>
{

    public async Task<Result<StudentAttendanceDisplay>> Handle(GetStudentAttendancesQuery request, CancellationToken cancellationToken)
    {
        return await attendnceRepository.GetAttendances(request.SessionId, applicationContext.TenantId);
    }

}
