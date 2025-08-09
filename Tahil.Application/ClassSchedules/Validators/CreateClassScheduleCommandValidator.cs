using Tahil.Application.ClassSchedules.Commands;

namespace Tahil.Application.ClassSchedules.Validators;

public class CreateClassScheduleCommandValidator : ClassScheduleCommandValidator<CreateClassScheduleCommand>
{
    public CreateClassScheduleCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
}