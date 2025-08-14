using Tahil.Application.ClassSessions.Validators;

namespace Tahil.Application.ClassSessions.Commands;

public record UpdateClassSessionCommand(ClassSessionDto Session) : IClassSessionCommand, ICommand<Result<bool>>;

public class UpdateClassSessionCommandHandler(
    IUnitOfWork unitOfWork,
    IClassSessionRepository classSessionRepository,
    IApplicationContext applicationContext,
    LocalizedStrings locale) : ICommandHandler<UpdateClassSessionCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateClassSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await classSessionRepository.GetAsync(r => r.Id == request.Session.Id && r.TenantId == applicationContext.TenantId);
        if (session == null)
            return Result<bool>.Failure(locale.NotAvailableClassSession);

        session!.Update(request.Session, applicationContext.UserName);
     
        var result = await unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(result);
    }
}
