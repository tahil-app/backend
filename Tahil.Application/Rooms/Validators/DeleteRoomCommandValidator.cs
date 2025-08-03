using FluentValidation;
using Tahil.Application.Rooms.Commands;
using Tahil.Domain.Localization;

namespace Tahil.Application.Rooms.Validators;

public class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
} 