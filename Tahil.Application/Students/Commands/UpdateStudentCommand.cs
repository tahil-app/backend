namespace Tahil.Application.Students.Commands;

public record UpdateStudentCommand(StudentDto Student) : ICommand<Result<bool>>;

public class UpdateStudentCommandHandler(IUnitOfWork unitOfWork, IStudentRepository studentRepository) : ICommandHandler<UpdateStudentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetAsync(r => r.Id == request.Student.Id);
        if (student is null)
            throw new NotFoundException("Student");

        student.Update(request.Student);

        if (request.Student.Password is not null && !string.IsNullOrEmpty(request.Student.Password))
            student.User.UpdatePassword(request.Student.Password);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}