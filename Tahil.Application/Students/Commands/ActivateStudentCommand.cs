namespace Tahil.Application.Students.Commands;

public record ActivateStudentCommand(int Id) : ICommand<Result<bool>>;

public class ActivateStudentCommandHandler(IUnitOfWork unitOfWork, IStudentRepository studentRepository) : ICommandHandler<ActivateStudentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ActivateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetAsync(r => r.Id == request.Id);
        if (student is null)
            throw new NotFoundException("Student");

        student.Activate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}