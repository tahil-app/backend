namespace Tahil.Application.Rooms.Commands;

public record DeActivateRoomCommand(int Id) : ICommand<Result<bool>>;

public class DeActivateRoomCommandHandler(IUnitOfWork unitOfWork, IRoomRepository roomRepository, LocalizedStrings locale, IApplicationContext applicationContext) : ICommandHandler<DeActivateRoomCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeActivateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetAsync(r => r.Id == request.Id && r.TenantId == applicationContext.TenantId);
        if (room is null)
            return Result<bool>.Failure(locale.NotAvailableRoom);

        room.DeActivate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}