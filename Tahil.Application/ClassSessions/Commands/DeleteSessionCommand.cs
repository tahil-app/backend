namespace Tahil.Application.ClassSessions.Commands;

public record DeleteSessionCommand(int Id) : ICommand<Result<bool>>;

public class DeleteClassSessionCommandHandler(IUnitOfWork unitOfWork, IClassSessionRepository classSessionRepository, IApplicationContext applicationContext) : ICommandHandler<DeleteSessionCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await classSessionRepository.DeleteSessionAsync(request.Id, applicationContext.TenantId);
        if (deleteResult.IsSuccess)
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return deleteResult;
    }
}