namespace Tahil.Application.Students.Commands;

public record DeleteStudentCommand(int Id) : ICommand<Result<bool>>;

public class DeleteStudentCommandHandler(IUnitOfWork unitOfWork, IStudentRepository groupRepository, IApplicationContext applicationContext) : ICommandHandler<DeleteStudentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await groupRepository.DeleteStudentAsync(request.Id, applicationContext.TenantId);
        if (deleteResult.IsSuccess)
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return deleteResult;
    }
}