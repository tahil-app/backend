using Tahil.Application.Teachers.Mappings;
using Tahil.Application.Teachers.Validators;

namespace Tahil.Application.Teachers.Commands;

public record CreateTeacherCommand(TeacherDto Teacher) : ICommand<Result<bool>>, ITeacherCommand;

public class CreateTeacherCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository teacherRepository, IApplicationContext applicationContext) : ICommandHandler<CreateTeacherCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = request.Teacher.ToTeacher();
        teacher.User.UpdatePassword(teacher.User.Password);
        teacher.User.SetRole(UserRole.Teacher);

        var result = await teacherRepository.AddTeacherAsync(teacher, applicationContext.TenantId);

        if (result.IsSuccess)
        {
            var saveResult = await unitOfWork.SaveChangesAsync();
            return Result.Success(saveResult);
        }

        return result;
    }
}