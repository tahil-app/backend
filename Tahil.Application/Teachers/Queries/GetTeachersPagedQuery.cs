namespace Tahil.Application.Teachers.Queries;

public record GetTeachersPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<TeacherDto>>>;

public class GetTeachersPagedQueryHandler(ITeacherRepository teacherRepository) : IQueryHandler<GetTeachersPagedQuery, Result<PagedList<TeacherDto>>>
{
    public async Task<Result<PagedList<TeacherDto>>> Handle(GetTeachersPagedQuery request, CancellationToken cancellationToken)
    {
        var teachers = await teacherRepository.GetPagedAsync(request.QueryParams, r => r.User.Role == UserRole.Teacher);
        var pagedTeachers = teachers.Adapt<PagedList<TeacherDto>>();

        return Result.Success(pagedTeachers);
    }
}