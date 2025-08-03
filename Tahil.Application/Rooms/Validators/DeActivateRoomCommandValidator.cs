using Tahil.Application.Rooms.Commands;

namespace Tahil.Application.Rooms.Validators;

public class DeActivateRoomCommandValidator : AbstractValidator<DeActivateRoomCommand>
{
    public DeActivateRoomCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
} 