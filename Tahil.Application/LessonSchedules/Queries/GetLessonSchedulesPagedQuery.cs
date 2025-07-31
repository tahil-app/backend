namespace Tahil.Application.LessonSchedules.Queries;

public record GetLessonSchedulesPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<LessonScheduleDto>>>;

public class GetLessonSchedulesPagedQueryHandler(ILessonScheduleRepository lessonScheduleRepository) : IQueryHandler<GetLessonSchedulesPagedQuery, Result<PagedList<LessonScheduleDto>>>
{
    public async Task<Result<PagedList<LessonScheduleDto>>> Handle(GetLessonSchedulesPagedQuery request, CancellationToken cancellationToken)
    {
        var schedules = await lessonScheduleRepository.GetPagedAsync(request.QueryParams);
        var pagedSchedules = schedules.Adapt<PagedList<LessonScheduleDto>>();

        return Result.Success(pagedSchedules);
    }
}