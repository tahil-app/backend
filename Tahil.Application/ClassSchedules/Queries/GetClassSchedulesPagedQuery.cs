//namespace Tahil.Application.ClassSchedules.Queries;

//public record GetClassSchedulesPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<ClassScheduleDto>>>;

//public class GetClassSchedulesPagedQueryHandler(IClassScheduleRepository ClassScheduleRepository) : IQueryHandler<GetClassSchedulesPagedQuery, Result<PagedList<ClassScheduleDto>>>
//{
//    public async Task<Result<PagedList<ClassScheduleDto>>> Handle(GetClassSchedulesPagedQuery request, CancellationToken cancellationToken)
//    {
//        var schedules = await ClassScheduleRepository.GetPagedAsync(request.QueryParams, includes: [r => r.Group, r => r.Room, r => r.Course, r => r.Teacher]);
//        var pagedSchedules = schedules.Adapt<PagedList<ClassScheduleDto>>();

//        return Result.Success(pagedSchedules);
//    }
//}