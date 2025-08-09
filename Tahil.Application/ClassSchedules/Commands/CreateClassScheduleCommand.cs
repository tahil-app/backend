using Tahil.Application.ClassSchedules.Mappings;
using Tahil.Application.ClassSchedules.Validators;

namespace Tahil.Application.ClassSchedules.Commands;

public record CreateClassScheduleCommand(ClassScheduleDto Schedule) : IClassScheduleCommand, ICommand<Result<bool>>;

public class CreateClassScheduleCommandHandler(IUnitOfWork unitOfWork, IClassScheduleRepository scheduleRepository, IApplicationContext applicationContext) : ICommandHandler<CreateClassScheduleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateClassScheduleCommand request, CancellationToken cancellationToken)
    {
        var addResult = await scheduleRepository.AddScheduleAsync(request.Schedule.ToClassSchedule(), applicationContext.TenantId);

        if (addResult.IsSuccess) 
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return addResult;
    }
}
