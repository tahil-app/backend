using Tahil.Application.Teachers.Commands;

namespace Tahil.Application.Teachers.Validators;

public class UpdateTeacherCommandValidator : TeacherCommandValidator<UpdateTeacherCommand>
{
    public UpdateTeacherCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
} 