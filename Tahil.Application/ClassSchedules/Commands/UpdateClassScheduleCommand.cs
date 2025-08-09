//using Tahil.Application.ClassSchedules.Validators;
//using Tahil.Domain.Localization;

//namespace Tahil.Application.ClassSchedules.Commands;

//public record UpdateClassScheduleCommand(ClassScheduleDto Schedule) : IClassScheduleCommand, ICommand<Result<bool>>;

//public class UpdateClassScheduleCommandHandler(
//    IUnitOfWork unitOfWork, 
//    IClassScheduleRepository ClassScheduleRepository, 
//    IApplicationContext applicationContext,
//    LocalizedStrings locale) : ICommandHandler<UpdateClassScheduleCommand, Result<bool>>
//{
//    public async Task<Result<bool>> Handle(UpdateClassScheduleCommand request, CancellationToken cancellationToken)
//    {
//        var ClassSchedule = await ClassScheduleRepository.GetAsync(r => r.Id == request.Schedule.Id);
//        if (ClassSchedule is null)
//            throw new NotFoundException(locale.NotFoundClassSchedule);

//        ClassSchedule.Update(request.Schedule, applicationContext.UserName);

//        // if start date or end date is different create a new one and link it with old.
//        // if we have created sessions, should create a new one.

//        var result = await unitOfWork.SaveChangesAsync();

//        return Result.Success(result);
//    }
//}
