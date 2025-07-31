namespace Tahil.Application.LessonSchedules.Queries;

public record GetLessonScheduleQuery(int Id) : IQuery<Result<LessonScheduleDto>>;

public class GetLessonScheduleQueryHandler(ILessonScheduleRepository lessonScheduleRepository) : IQueryHandler<GetLessonScheduleQuery, Result<LessonScheduleDto>>
{
    public async Task<Result<LessonScheduleDto>> Handle(GetLessonScheduleQuery request, CancellationToken cancellationToken)
    {
        var scheduleDto = await lessonScheduleRepository.GetAsync(r => r.Id == request.Id);
        return Result.Success(scheduleDto.Adapt<LessonScheduleDto>());
    }
}