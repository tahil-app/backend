using Tahil.Application.Rooms.Mappings;
using Tahil.Application.Rooms.Validators;

namespace Tahil.Application.Rooms.Commands;

public record CreateRoomCommand(RoomDto RoomDto) : ICommand<Result<bool>>, IRoomCommand
{
    public RoomDto Room => RoomDto;
}

public class CreateRoomCommandHandler(IUnitOfWork unitOfWork, IRoomRepository roomRepository, IApplicationContext applicationContext) : ICommandHandler<CreateRoomCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        await roomRepository.AddRoomAsync(request.RoomDto.ToRoom(), applicationContext.TenantId);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}