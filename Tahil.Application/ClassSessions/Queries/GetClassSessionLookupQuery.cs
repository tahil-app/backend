namespace Tahil.Application.ClassSessions.Queries;

public record GetClassSessionLookupQuery(int CourseId) : IQuery<Result<ClassSessionLookupsDto>>;

public class GetClassSessionLookupQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetClassSessionLookupQuery, Result<ClassSessionLookupsDto>>
{
    public async Task<Result<ClassSessionLookupsDto>> Handle(GetClassSessionLookupQuery request, CancellationToken cancellationToken)
    {
        var lookupsDto = await lookupRepository.GetClassSessionAsync(applicationContext.TenantId, request.CourseId);
        return Result.Success(lookupsDto);
    }
}