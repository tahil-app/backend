namespace Tahil.Application.ClassSessions.Commands;

public record UpdateClassSessionStatusCommand(int SessionId, ClassSessionStatus Status) : ICommand<Result<bool>>;

public class UpdateClassSessionStatusCommandHandler(
    IUnitOfWork unitOfWork,
    IClassSessionRepository classSessionRepository,
    IApplicationContext applicationContext,
    LocalizedStrings locale) : ICommandHandler<UpdateClassSessionStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateClassSessionStatusCommand request, CancellationToken cancellationToken)
    {
        var session = await classSessionRepository.GetAsync(r => r.Id == request.SessionId && r.TenantId == applicationContext.TenantId);
        if (session == null)
            return Result<bool>.Failure(locale.NotAvailableClassSession);

        session!.UpdateStatus(request.Status, applicationContext.UserName);
     
        var result = await unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(result);
    }
}
