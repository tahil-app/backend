using Tahil.Application.Courses.Commands;

namespace Tahil.Application.Courses.Validators;

public class DeActivateCourseCommandValidator : AbstractValidator<DeActivateCourseCommand>
{
    public DeActivateCourseCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
} 