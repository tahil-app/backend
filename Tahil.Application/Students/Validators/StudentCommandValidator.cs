namespace Tahil.Application.Students.Validators;

public interface IStudentCommand
{
    StudentDto Student { get; }
}

public abstract class StudentCommandValidator<T> : AbstractValidator<T>
    where T : IStudentCommand
{

    private readonly LocalizedStrings locale;
    protected StudentCommandValidator(LocalizedStrings locale)
    {
        this.locale = locale;
    }

    public void AddCommonRules()
    {
        RuleFor(x => x.Student)
            .NotNull()
            .WithMessage(locale.CannotBeNull);

        When(x => x.Student != null, () =>
        {
            RuleFor(x => x.Student.Name)
                .NotEmpty()
                .WithMessage(locale.RequiredName)
                .MinimumLength(2)
                .WithMessage(locale.StudentNameTooShort)
                .MaximumLength(100)
                .WithMessage(locale.StudentNameTooLong);

            RuleFor(x => x.Student.Email)
                .NotEmpty()
                .WithMessage(locale.RequiredEmail)
                .EmailAddress()
                .WithMessage(locale.InvalidEmailFormat);

            RuleFor(x => x.Student.PhoneNumber)
                .NotEmpty()
                .WithMessage(locale.RequiredPhoneNumber);

            RuleFor(x => x.Student.Qualification)
                .MaximumLength(200)
                .WithMessage(locale.StudentQualificationTooLong);
        });
    }
} 