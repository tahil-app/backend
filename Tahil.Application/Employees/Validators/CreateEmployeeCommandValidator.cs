using Tahil.Application.Employees.Commands;

namespace Tahil.Application.Employees.Validators;

public class CreateEmployeeCommandValidator : EmployeeCommandValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();

        RuleFor(x => x.Employee.Password)
                .NotEmpty()
                .WithMessage(locale.RequiredPassword);
    }
} 