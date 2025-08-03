using Tahil.Application.LessonSchedules.Commands;
using Tahil.Domain.Localization;

namespace Tahil.Application.LessonSchedules.Validators;

public class CreateLessonScheduleCommandValidator : LessonScheduleCommandValidator<CreateLessonScheduleCommand>
{
    public CreateLessonScheduleCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
}