namespace Tahil.Application.ClassSchedules.Validators;

public interface IClassScheduleCommand
{
    ClassScheduleDto Schedule { get; }
}

public abstract class ClassScheduleCommandValidator<T> : AbstractValidator<T>
    where T : IClassScheduleCommand
{

    private readonly LocalizedStrings locale;
    protected ClassScheduleCommandValidator(LocalizedStrings locale)
    {
        this.locale = locale;
    }

    public void AddCommonRules()
    {
        RuleFor(x => x.Schedule.GroupId).NotNull().WithMessage(locale.RequiredGroup);
        RuleFor(x => x.Schedule.RoomId).NotNull().WithMessage(locale.RequiredRoom);
        RuleFor(x => x.Schedule.Day).NotNull().WithMessage(locale.RequiredDay);
        RuleFor(x => x.Schedule.StartTime).NotNull().WithMessage(locale.RequiredStartTime);
        RuleFor(x => x.Schedule.EndTime).NotNull().WithMessage(locale.RequiredEndTime);

        RuleFor(x => x.Schedule)
            .Must(x => x.StartTime == null || x.EndTime == null || x.StartTime < x.EndTime)
            .WithMessage(locale.StartTimeMustBeBeforeEndTime);

        RuleFor(x => x.Schedule)
            .Must(x => x.StartDate == null || x.EndDate == null || x.StartDate < x.EndDate)
            .WithMessage(locale.StartDateMustBeBeforeEndDate);
    }
}