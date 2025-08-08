namespace Tahil.Application.Students.Queries;

public record GetStudentsPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<StudentDto>>>;

public class GetStudentsPagedQueryHandler(IStudentRepository studentRepository, IApplicationContext applicationContext) : IQueryHandler<GetStudentsPagedQuery, Result<PagedList<StudentDto>>>
{
    public async Task<Result<PagedList<StudentDto>>> Handle(GetStudentsPagedQuery request, CancellationToken cancellationToken)
    {
        var students = await studentRepository.GetPagedAsync(request.QueryParams, r => r.User.Role == UserRole.Student && r.User.TenantId == applicationContext.TenantId);
        var pagedStudents = students.Adapt<PagedList<StudentDto>>();

        return Result.Success(pagedStudents);
    }
}