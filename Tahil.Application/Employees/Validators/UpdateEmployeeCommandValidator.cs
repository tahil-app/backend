using Tahil.Application.Employees.Commands;

namespace Tahil.Application.Employees.Validators;

public class UpdateEmployeeCommandValidator : EmployeeCommandValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
} 