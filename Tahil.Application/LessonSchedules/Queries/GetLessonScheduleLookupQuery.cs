namespace Tahil.Application.LessonSchedules.Queries;

public record GetLessonScheduleLookupQuery() : IQuery<Result<LessonScheduleLookupsDto>>;

public class GetLessonScheduleLookupQueryHandler(ILookupRepository lookupRepository) : IQueryHandler<GetLessonScheduleLookupQuery, Result<LessonScheduleLookupsDto>>
{
    public async Task<Result<LessonScheduleLookupsDto>> Handle(GetLessonScheduleLookupQuery request, CancellationToken cancellationToken)
    {
        var lookupsDto = await lookupRepository.GetLessonScheduleLookupsAsync();
        return Result.Success(lookupsDto);
    }
}