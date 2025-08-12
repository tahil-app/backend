using Tahil.Application.ClassSchedules.Mappings;
using Tahil.Application.ClassSchedules.Validators;

namespace Tahil.Application.ClassSchedules.Commands;

public record UpdateClassScheduleCommand(ClassScheduleDto Schedule) : IClassScheduleCommand, ICommand<Result<bool>>;

public class UpdateClassScheduleCommandHandler(
    IUnitOfWork unitOfWork,
    IClassScheduleRepository ClassScheduleRepository,
    IApplicationContext applicationContext) : ICommandHandler<UpdateClassScheduleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateClassScheduleCommand request, CancellationToken cancellationToken)
    {
        var updateResult = await ClassScheduleRepository.UpdateScheduleAsync(request.Schedule.ToClassSchedule(), applicationContext.TenantId);

        if (updateResult.IsSuccess) 
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return updateResult;
    }
}
