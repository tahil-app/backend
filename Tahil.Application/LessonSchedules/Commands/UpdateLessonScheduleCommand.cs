using Tahil.Application.LessonSchedules.Validators;
using Tahil.Domain.Localization;

namespace Tahil.Application.LessonSchedules.Commands;

public record UpdateLessonScheduleCommand(LessonScheduleDto Schedule) : ILessonScheduleCommand, ICommand<Result<bool>>;

public class UpdateLessonScheduleCommandHandler(
    IUnitOfWork unitOfWork, 
    ILessonScheduleRepository lessonScheduleRepository, 
    IApplicationContext applicationContext,
    LocalizedStrings locale) : ICommandHandler<UpdateLessonScheduleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateLessonScheduleCommand request, CancellationToken cancellationToken)
    {
        var lessonSchedule = await lessonScheduleRepository.GetAsync(r => r.Id == request.Schedule.Id);
        if (lessonSchedule is null)
            throw new NotFoundException(locale.NotFoundLessonSchedule);

        lessonSchedule.Update(request.Schedule, applicationContext.UserName);

        // if start date or end date is different create a new one and link it with old.
        // if we have created sessions, should create a new one.

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}
