using Tahil.Application.Teachers.Commands;

namespace Tahil.Application.Teachers.Validators;

public class DeActivateTeacherCommandValidator : AbstractValidator<DeActivateTeacherCommand>
{
    public DeActivateTeacherCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
} 