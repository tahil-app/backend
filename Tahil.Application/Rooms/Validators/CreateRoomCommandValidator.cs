using Tahil.Application.Rooms.Commands;

namespace Tahil.Application.Rooms.Validators;

public class CreateRoomCommandValidator : RoomCommandValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
} 