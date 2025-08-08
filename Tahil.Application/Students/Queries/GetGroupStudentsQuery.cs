namespace Tahil.Application.Students.Queries;

public record GetGroupStudentsQueryQuery(int GroupId) : IQuery<Result<List<StudentDto>>>;

public class GetGroupStudentsQueryQueryHandler(IStudentRepository studentRepository, IApplicationContext applicationContext) : IQueryHandler<GetGroupStudentsQueryQuery, Result<List<StudentDto>>>
{
    public async Task<Result<List<StudentDto>>> Handle(GetGroupStudentsQueryQuery request, CancellationToken cancellationToken)
    {
        var students = await studentRepository.GetAllReadOnlyAsync(r => r.User.TenantId == applicationContext.TenantId && r.StudentGroups.Any(sg => sg.GroupId == request.GroupId));
        var pagedStudents = students.Adapt<List<StudentDto>>();

        return Result.Success(pagedStudents);
    }
} 