using Tahil.Application.ClassSchedules.Commands;

namespace Tahil.Application.ClassSchedules.Validators;

public class UpdateClassScheduleCommandValidator : ClassScheduleCommandValidator<UpdateClassScheduleCommand>
{
    public UpdateClassScheduleCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
}