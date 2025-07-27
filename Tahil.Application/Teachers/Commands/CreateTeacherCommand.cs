using Tahil.Application.Teachers.Mappings;

namespace Tahil.Application.Teachers.Commands;

public record CreateTeacherCommand(TeacherDto Teacher) : ICommand<Result<bool>>;

public class CreateTeacherCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository teacherRepository) : ICommandHandler<CreateTeacherCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = request.Teacher.ToTeacher();
        teacher.User.UpdatePassword(teacher.User.Password);
        teacher.User.SetRole(UserRole.Teacher);

        await teacherRepository.AddTeacherAsync(teacher);

        var result = await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(result);
    }
}