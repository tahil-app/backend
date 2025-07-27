namespace Tahil.Application.Rooms.Commands;

public record ActivateRoomCommand(int Id) : ICommand<Result<bool>>;

public class ActivateRoomCommandHandler(IUnitOfWork unitOfWork, IRoomRepository roomRepository) : ICommandHandler<ActivateRoomCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ActivateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetAsync(r => r.Id == request.Id);
        if (room is null)
            throw new NotFoundException("Room");

        room.Activate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}