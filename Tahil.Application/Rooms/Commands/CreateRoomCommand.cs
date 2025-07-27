using Tahil.Application.Rooms.Mappings;

namespace Tahil.Application.Rooms.Commands;

public record CreateRoomCommand(RoomDto RoomDto) : ICommand<Result<bool>>;

public class CreateRoomCommandHandler(IUnitOfWork unitOfWork, IRoomRepository roomRepository) : ICommandHandler<CreateRoomCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        await roomRepository.AddRoomAsync(request.RoomDto.ToRoom());

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}