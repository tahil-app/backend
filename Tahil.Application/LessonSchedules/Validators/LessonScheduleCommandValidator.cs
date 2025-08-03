using Tahil.Domain.Localization;

namespace Tahil.Application.LessonSchedules.Validators;

public interface ILessonScheduleCommand
{
    LessonScheduleDto Schedule { get; }
}

public abstract class LessonScheduleCommandValidator<T> : AbstractValidator<T>
    where T : ILessonScheduleCommand
{

    private readonly LocalizedStrings locale;
    protected LessonScheduleCommandValidator(LocalizedStrings locale)
    {
       this.locale = locale;
    }

    public void AddCommonRules() 
    {
        RuleFor(x => x.Schedule.GroupId).NotNull().WithMessage(locale.RequiredGroup);
        RuleFor(x => x.Schedule.RoomId).NotNull().WithMessage(locale.RequiredRoom);
        RuleFor(x => x.Schedule.CourseId).NotNull().WithMessage(locale.RequiredCourse);
        RuleFor(x => x.Schedule.TeacherId).NotNull().WithMessage(locale.RequiredTeacher);
        RuleFor(x => x.Schedule.StartDate).NotNull().WithMessage(locale.RequiredStartDate);
        RuleFor(x => x.Schedule.EndDate).NotNull().WithMessage(locale.RequiredEndDate);

        RuleFor(x => x.Schedule)
            .Must(x => x.StartDate <= x.EndDate)
            .WithMessage(locale.StartAndEndDatesValidation);
    }
}