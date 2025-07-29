using Tahil.Application.Students.Mappings;
using Tahil.Domain.Entities;

namespace Tahil.Application.Students.Commands;

public record CreateStudentCommand(StudentDto Student) : ICommand<Result<bool>>;

public class CreateStudentCommandHandler(IUnitOfWork unitOfWork, IStudentRepository studentRepository) : ICommandHandler<CreateStudentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = request.Student.ToStudent();
        student.User.UpdatePassword(student.User.Password);
        student.User.SetRole(UserRole.Student);

        if (request.Student.Groups.Any())
            student.UpdateGroups(request.Student.Groups.Adapt<List<Group>>());

        await studentRepository.AddStudentAsync(student);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}