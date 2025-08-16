using Tahil.Domain.Entities;

namespace Tahil.Application.StudentAttendancs.Commands;

public record UpdateStudentAttendanceCommand(int SessionId, List<StudentAttendanceDto> StudentAttendances) : ICommand<Result<bool>>;

public class UpdateSessionAttendanceCommandHandler(
    IClassSessionRepository classSessionRepository, IApplicationContext applicationContext, IUnitOfWork unitOfWork, LocalizedStrings locale)
    : ICommandHandler<UpdateStudentAttendanceCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateStudentAttendanceCommand request, CancellationToken cancellationToken)
    {
        var session = await classSessionRepository.GetAsync(r => r.Id == request.SessionId && r.TenantId == applicationContext.TenantId, [r => r.StudentAttendances]);
        if (session == null)
        {
            return Result<bool>.Failure(locale.NotAvailableClassSession);
        }

        session!.UpdateStudentAttendance(request.StudentAttendances.Adapt<List<StudentAttendance>>(), applicationContext.UserName);

        var result = await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(result);
    }
}