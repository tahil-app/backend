namespace Tahil.Application.ClassSchedules.Queries;

public record GetClassScheduleLookupQuery() : IQuery<Result<ClassScheduleLookupsDto>>;

public class GetClassScheduleLookupQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetClassScheduleLookupQuery, Result<ClassScheduleLookupsDto>>
{
    public async Task<Result<ClassScheduleLookupsDto>> Handle(GetClassScheduleLookupQuery request, CancellationToken cancellationToken)
    {
        var lookupsDto = await lookupRepository.GetClassScheduleAsync(applicationContext.TenantId);
        return Result.Success(lookupsDto);
    }
}