using Tahil.Application.Students.Commands;

namespace Tahil.Application.Students.Validators;

public class DeActivateStudentCommandValidator : AbstractValidator<DeActivateStudentCommand>
{
    public DeActivateStudentCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
} 