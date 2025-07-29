namespace Tahil.Application.Groups.Commands;

public record DeleteGroupCommand(int GroupId) : ICommand<Result<bool>>;

public class DeleteGroupCommandHandler(IUnitOfWork unitOfWork, IGroupRepository groupRepository) : ICommandHandler<DeleteGroupCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        await groupRepository.DeleteGroupAsync(request.GroupId);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}