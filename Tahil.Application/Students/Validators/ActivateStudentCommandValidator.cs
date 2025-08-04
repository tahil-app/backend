using Tahil.Application.Students.Commands;

namespace Tahil.Application.Students.Validators;

public class ActivateStudentCommandValidator : AbstractValidator<ActivateStudentCommand>
{
    public ActivateStudentCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
} 