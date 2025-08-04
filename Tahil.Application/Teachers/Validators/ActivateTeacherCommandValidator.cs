using Tahil.Application.Teachers.Commands;

namespace Tahil.Application.Teachers.Validators;

public class ActivateTeacherCommandValidator : AbstractValidator<ActivateTeacherCommand>
{
    public ActivateTeacherCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
} 