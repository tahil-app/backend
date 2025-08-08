namespace Tahil.Application.Students.Queries;

public record GetAllStudentsQuery() : IQuery<Result<List<StudentDto>>>;

public class GetAllStudentsQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllStudentsQuery, Result<List<StudentDto>>>
{
    public async Task<Result<List<StudentDto>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        return await lookupRepository.GetStudentsAsync(applicationContext.TenantId);
    }
}