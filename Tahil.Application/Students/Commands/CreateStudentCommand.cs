using Tahil.Application.Students.Mappings;
using Tahil.Application.Students.Validators;
using Tahil.Domain.Entities;

namespace Tahil.Application.Students.Commands;

public record CreateStudentCommand(StudentDto Student) : ICommand<Result<bool>>, IStudentCommand;

public class CreateStudentCommandHandler(IUnitOfWork unitOfWork, IStudentRepository studentRepository, IApplicationContext applicationContext) : ICommandHandler<CreateStudentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = request.Student.ToStudent();
        student.User.UpdatePassword(student.User.Password);
        student.User.SetRole(UserRole.Student);

        if (request.Student.Groups is not null && request.Student.Groups.Any())
            student.UpdateGroups(request.Student.Groups.Adapt<List<Group>>());

        var result = await studentRepository.AddStudentAsync(student, applicationContext.TenantId);

        if (result.IsSuccess)
        {
            var added = await unitOfWork.SaveChangesAsync();
            return Result.Success(added);
        }

        return result;
    }
}