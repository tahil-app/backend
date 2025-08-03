using Tahil.Application.Rooms.Commands;

namespace Tahil.Application.Rooms.Validators;

public class UpdateRoomCommandValidator : RoomCommandValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
} 