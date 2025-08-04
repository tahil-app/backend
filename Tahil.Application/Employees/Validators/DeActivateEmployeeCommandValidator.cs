using Tahil.Application.Employees.Commands;

namespace Tahil.Application.Employees.Validators;

public class DeActivateEmployeeCommandValidator : AbstractValidator<DeActivateEmployeeCommand>
{
    public DeActivateEmployeeCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
} 