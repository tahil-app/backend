using Tahil.Application.ClassSessions.Commands;

namespace Tahil.Application.ClassSessions.Validators;

public class UpdateClassSessionCommandValidator : ClassSessionCommandValidator<UpdateClassSessionCommand>
{
    public UpdateClassSessionCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
}