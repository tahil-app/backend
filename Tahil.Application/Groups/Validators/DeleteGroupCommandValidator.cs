using Tahil.Application.Groups.Commands;

namespace Tahil.Application.Groups.Validators;

public class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
{
    public DeleteGroupCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
}