namespace Tahil.Application.Students.Commands;

public record DeActivateStudentCommand(int Id) : ICommand<Result<bool>>;

public class DeActivateStudentCommandHandler(IUnitOfWork unitOfWork, IStudentRepository studentRepository, LocalizedStrings locale) : ICommandHandler<DeActivateStudentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeActivateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetAsync(r => r.Id == request.Id);
        if (student is null)
            return Result<bool>.Failure(locale.NotAvailableStudent);

        student.DeActivate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}
