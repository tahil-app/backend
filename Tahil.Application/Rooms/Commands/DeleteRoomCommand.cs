namespace Tahil.Application.Rooms.Commands;

public record DeleteRoomCommand(int Id) : ICommand<Result<bool>>;

public class DeleteRoomCommandHandler(IUnitOfWork unitOfWork, IRoomRepository groupRepository) : ICommandHandler<DeleteRoomCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await groupRepository.DeleteRoomAsync(request.Id);
        if (deleteResult.IsSuccess)
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return deleteResult;
    }
}