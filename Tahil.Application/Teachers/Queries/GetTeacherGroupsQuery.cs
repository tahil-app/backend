namespace Tahil.Application.Teachers.Queries;

public record GetTeacherGroupsQuery(int Id) : IQuery<Result<List<GroupDto>>>;

public class GetTeacherGroupsQueryHandler(ITeacherRepository teacherRepository, IApplicationContext applicationContext) : IQueryHandler<GetTeacherGroupsQuery, Result<List<GroupDto>>>
{
    public async Task<Result<List<GroupDto>>> Handle(GetTeacherGroupsQuery request, CancellationToken cancellationToken)
    {
        var teacherGroups = await teacherRepository.GetTeacherGroupsAsync(request.Id, applicationContext.TenantId);
        return Result.Success(teacherGroups);
    }
}
