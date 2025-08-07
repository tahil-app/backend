using Tahil.Application.Rooms.Validators;

namespace Tahil.Application.Rooms.Commands;

public record UpdateRoomCommand(RoomDto RoomDto) : ICommand<Result<bool>>, IRoomCommand
{
    public RoomDto Room => RoomDto;
}

public class UpdateRoomCommandHandler(IUnitOfWork unitOfWork, IRoomRepository roomRepository, LocalizedStrings locale, IApplicationContext applicationContext) : ICommandHandler<UpdateRoomCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetAsync(r => r.Id == request.RoomDto.Id && r.TenantId == applicationContext.TenantId);
        if (room == null)
            return Result<bool>.Failure(locale.NotAvailableRoom);

        room.Update(request.RoomDto);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}