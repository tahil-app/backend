namespace Tahil.Application.Employees.Validators;

public interface IEmployeeCommand
{
    UserDto Employee { get; }
}

public abstract class EmployeeCommandValidator<T> : AbstractValidator<T>
    where T : IEmployeeCommand
{

    private readonly LocalizedStrings locale;
    protected EmployeeCommandValidator(LocalizedStrings locale)
    {
        this.locale = locale;
    }

    public void AddCommonRules()
    {
        RuleFor(x => x.Employee)
            .NotNull()
            .WithMessage(locale.CannotBeNull);

        When(x => x.Employee != null, () =>
        {
            RuleFor(x => x.Employee.Name)
                .NotEmpty()
                .WithMessage(locale.RequiredName)
                .MinimumLength(2)
                .WithMessage(locale.EmployeeNameTooShort)
                .MaximumLength(100)
                .WithMessage(locale.EmployeeNameTooLong);

            RuleFor(x => x.Employee.Email)
                .NotEmpty()
                .WithMessage(locale.RequiredEmail)
                .EmailAddress()
                .WithMessage(locale.InvalidEmailFormat);

            RuleFor(x => x.Employee.PhoneNumber)
                .NotEmpty()
                .WithMessage(locale.RequiredPhoneNumber);
        });
    }
} 