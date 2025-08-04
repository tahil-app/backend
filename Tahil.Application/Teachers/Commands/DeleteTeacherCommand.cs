namespace Tahil.Application.Teachers.Commands;

public record DeleteTeacherCommand(int Id) : ICommand<Result<bool>>;

public class DeleteTeacherCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository groupRepository) : ICommandHandler<DeleteTeacherCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await groupRepository.DeleteTeacherAsync(request.Id);
        if (deleteResult.IsSuccess)
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return deleteResult;
    }
}