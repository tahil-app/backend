namespace Tahil.Application.Teachers.Queries;

public record GetAllTeachersQuery() : IQuery<Result<List<TeacherDto>>>;

public class GetAllTeachersQueryHandler(ITeacherRepository teacherRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllTeachersQuery, Result<List<TeacherDto>>>
{
    public async Task<Result<List<TeacherDto>>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
    {
        var teachers = await teacherRepository.GetAllReadOnlyAsync(r => r.User.IsActive && r.User.TenantId == applicationContext.TenantId);
        var activeTeachers = teachers.Adapt<List<TeacherDto>>();

        return Result.Success(activeTeachers);
    }
}