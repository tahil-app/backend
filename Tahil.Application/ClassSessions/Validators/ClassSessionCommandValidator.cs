namespace Tahil.Application.ClassSessions.Validators;

public interface IClassSessionCommand
{
    ClassSessionDto Session { get; }
}

public abstract class ClassSessionCommandValidator<T> : AbstractValidator<T>
    where T : IClassSessionCommand
{

    private readonly LocalizedStrings locale;
    protected ClassSessionCommandValidator(LocalizedStrings locale)
    {
        this.locale = locale;
    }

    public void AddCommonRules()
    { 
        RuleFor(x => x.Session)
            .NotNull()
            .WithMessage(locale.CannotBeNull);

        RuleFor(x => x.Session.ScheduleId)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);

        RuleFor(x => x.Session.RoomId)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);

        RuleFor(x => x.Session.TeacherId)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);

        RuleFor(x => x.Session.StartTime)
            .NotEmpty()
            .WithMessage(locale.RequiredStartTime);

        RuleFor(x => x.Session.EndTime)
            .NotEmpty()
            .WithMessage(locale.RequiredEndTime);

        RuleFor(x => x.Session.Date)
            .NotEmpty()
            .WithMessage(locale.RequiredStartDate);

        RuleFor(x => x.Session)
            .Must(session => session.StartTime < session.EndTime)
            .WithMessage(locale.StartTimeMustBeBeforeEndTime);
    }
}

