namespace Tahil.Application.ClassSchedules.Commands;

public record DeleteClassScheduleCommand(int ScheduleId) : ICommand<Result<bool>>;

public class DeleteClassScheduleCommandHandler(
    IUnitOfWork unitOfWork,
    IClassScheduleRepository ClassScheduleRepository,
    IApplicationContext applicationContext) : ICommandHandler<DeleteClassScheduleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteClassScheduleCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await ClassScheduleRepository.DeleteScheduleAsync(request.ScheduleId, applicationContext.TenantId);

        if (deleteResult.IsSuccess) 
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return deleteResult;
    }
}
