namespace Tahil.Application.Teachers.Commands;

public record UpdateTeacherCommand(TeacherDto Teacher) : ICommand<Result<bool>>;

public class UpdateTeacherCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository teacherRepository) : ICommandHandler<UpdateTeacherCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.Teacher.Id);
        if (teacher is null)
            throw new NotFoundException("Teacher");

        teacher.Update(request.Teacher);

        if (request.Teacher.Password is not null && !string.IsNullOrEmpty(request.Teacher.Password))
            teacher.User.UpdatePassword(request.Teacher.Password);

        var result = await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}