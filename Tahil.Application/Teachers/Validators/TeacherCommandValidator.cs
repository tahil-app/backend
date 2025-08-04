namespace Tahil.Application.Teachers.Validators;

public interface ITeacherCommand
{
    TeacherDto Teacher { get; }
}

public abstract class TeacherCommandValidator<T> : AbstractValidator<T>
    where T : ITeacherCommand
{

    private readonly LocalizedStrings locale;
    protected TeacherCommandValidator(LocalizedStrings locale)
    {
        this.locale = locale;
    }

    public void AddCommonRules()
    {
        RuleFor(x => x.Teacher)
            .NotNull()
            .WithMessage(locale.CannotBeNull);

        When(x => x.Teacher != null, () =>
        {
            RuleFor(x => x.Teacher.Name)
                .NotEmpty()
                .WithMessage(locale.RequiredName)
                .MinimumLength(2)
                .WithMessage(locale.TeacherNameTooShort)
                .MaximumLength(100)
                .WithMessage(locale.TeacherNameTooLong);

            RuleFor(x => x.Teacher.Email)
                .NotEmpty()
                .WithMessage(locale.RequiredEmail)
                .EmailAddress()
                .WithMessage(locale.InvalidEmailFormat);

            RuleFor(x => x.Teacher.PhoneNumber)
                .NotEmpty()
                .WithMessage(locale.RequiredPhoneNumber);

            RuleFor(x => x.Teacher.Experience)
                .MaximumLength(500)
                .WithMessage(locale.TeacherExperienceTooLong);

            RuleFor(x => x.Teacher.Qualification)
                .MaximumLength(200)
                .WithMessage(locale.TeacherQualificationTooLong);
        });
    }
} 