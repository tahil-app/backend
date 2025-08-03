using Tahil.Application.Rooms.Commands;

namespace Tahil.Application.Rooms.Validators;

public class ActivateRoomCommandValidator : AbstractValidator<ActivateRoomCommand>
{
    public ActivateRoomCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
} 