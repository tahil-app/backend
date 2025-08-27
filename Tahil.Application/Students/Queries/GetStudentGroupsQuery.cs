namespace Tahil.Application.Students.Queries;

public record GetStudentGroupsQuery(int Id) : IQuery<Result<List<GroupDto>>>;

public class GetStudentGroupsQueryHandler(IStudentRepository studentRepository, IApplicationContext applicationContext) : IQueryHandler<GetStudentGroupsQuery, Result<List<GroupDto>>>
{
    public async Task<Result<List<GroupDto>>> Handle(GetStudentGroupsQuery request, CancellationToken cancellationToken)
    {
        var studentGroups = await studentRepository.GetStudentGroupsAsync(request.Id, applicationContext.TenantId);
        return Result.Success(studentGroups);
    }
}
