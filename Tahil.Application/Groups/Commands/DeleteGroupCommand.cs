namespace Tahil.Application.Groups.Commands;

public record DeleteGroupCommand(int Id) : ICommand<Result<bool>>;

public class DeleteGroupCommandHandler(IUnitOfWork unitOfWork, IGroupRepository groupRepository) : ICommandHandler<DeleteGroupCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await groupRepository.DeleteGroupAsync(request.Id);
        if (deleteResult.IsSuccess)
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return deleteResult;
    }
}