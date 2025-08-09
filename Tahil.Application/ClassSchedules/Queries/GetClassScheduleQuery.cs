//namespace Tahil.Application.ClassSchedules.Queries;

//public record GetClassScheduleQuery(int Id) : IQuery<Result<ClassScheduleDto>>;

//public class GetClassScheduleQueryHandler(IClassScheduleRepository ClassScheduleRepository) : IQueryHandler<GetClassScheduleQuery, Result<ClassScheduleDto>>
//{
//    public async Task<Result<ClassScheduleDto>> Handle(GetClassScheduleQuery request, CancellationToken cancellationToken)
//    {
//        var scheduleDto = await ClassScheduleRepository.GetAsync(r => r.Id == request.Id);
//        return Result.Success(scheduleDto.Adapt<ClassScheduleDto>());
//    }
//}