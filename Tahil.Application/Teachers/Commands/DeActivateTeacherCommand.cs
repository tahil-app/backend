namespace Tahil.Application.Teachers.Commands;

public record DeActivateTeacherCommand(int Id) : ICommand<Result<bool>>;

public class DeActivateTeacherCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository teacherRepository, LocalizedStrings locale, IApplicationContext applicationContext) : ICommandHandler<DeActivateTeacherCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeActivateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.Id && r.User.TenantId == applicationContext.TenantId);
        if (teacher is null)
            return Result<bool>.Failure(locale.NotAvailableTeacher);

        teacher.DeActivate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}