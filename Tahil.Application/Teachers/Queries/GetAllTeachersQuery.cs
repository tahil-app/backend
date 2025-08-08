namespace Tahil.Application.Teachers.Queries;

public record GetAllTeachersQuery() : IQuery<Result<List<TeacherDto>>>;

public class GetAllTeachersQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllTeachersQuery, Result<List<TeacherDto>>>
{
    public async Task<Result<List<TeacherDto>>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
    {
        return await lookupRepository.GetTeachersAsync(applicationContext.TenantId);
    }
}