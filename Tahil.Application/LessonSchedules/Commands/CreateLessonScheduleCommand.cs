using Tahil.Application.LessonSchedules.Mappings;
using Tahil.Application.LessonSchedules.Validators;

namespace Tahil.Application.LessonSchedules.Commands;

public record CreateLessonScheduleCommand(LessonScheduleDto Schedule) : ILessonScheduleCommand, ICommand<Result<bool>>;

public class CreateLessonScheduleCommandHandler(IUnitOfWork unitOfWork, ILessonScheduleRepository lessonScheduleRepository) : ICommandHandler<CreateLessonScheduleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateLessonScheduleCommand request, CancellationToken cancellationToken)
    {

        lessonScheduleRepository.Add(request.Schedule.ToLessonSchedule());

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}
