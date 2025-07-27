namespace Tahil.Application.Rooms.Commands;

public record UpdateRoomCommand(RoomDto RoomDto) : ICommand<Result<bool>>;

public class UpdateRoomCommandHandler(IUnitOfWork unitOfWork, IRoomRepository roomRepository) : ICommandHandler<UpdateRoomCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetAsync(r => r.Id == request.RoomDto.Id);
        if (room == null)
            throw new NotFoundException("Room");

        room.Update(request.RoomDto);

        var result = await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}