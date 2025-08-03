using Tahil.Application.Courses.Commands;

namespace Tahil.Application.Courses.Validators;

public class ActivateCourseCommandValidator : AbstractValidator<ActivateCourseCommand>
{
    public ActivateCourseCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
} 