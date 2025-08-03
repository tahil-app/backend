using Tahil.Application.LessonSchedules.Commands;
using Tahil.Domain.Localization;

namespace Tahil.Application.LessonSchedules.Validators;

public class UpdateLessonScheduleCommandValidator : LessonScheduleCommandValidator<UpdateLessonScheduleCommand>
{
    public UpdateLessonScheduleCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
}