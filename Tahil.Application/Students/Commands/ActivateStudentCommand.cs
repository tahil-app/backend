namespace Tahil.Application.Students.Commands;

public record ActivateStudentCommand(int Id) : ICommand<Result<bool>>;

public class ActivateStudentCommandHandler(IUnitOfWork unitOfWork, IStudentRepository studentRepository, LocalizedStrings locale, IApplicationContext applicationContext) : ICommandHandler<ActivateStudentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ActivateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetAsync(r => r.Id == request.Id && r.User.TenantId == applicationContext.TenantId);
        if (student is null)
            return Result<bool>.Failure(locale.NotAvailableStudent);

        student.Activate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}