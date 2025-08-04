using Tahil.Application.Teachers.Validators;
using Tahil.Domain.Entities;

namespace Tahil.Application.Teachers.Commands;

public record UpdateTeacherCommand(TeacherDto Teacher) : ICommand<Result<bool>>, ITeacherCommand;
public class UpdateTeacherCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository teacherRepository, LocalizedStrings locale) : ICommandHandler<UpdateTeacherCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.Teacher.Id, [r => r.TeacherCourses]);
        if (teacher is null)
            return Result<bool>.Failure(locale.NotAvailableTeacher);

        teacher.Update(request.Teacher);

        if (request.Teacher.Password is not null && !string.IsNullOrEmpty(request.Teacher.Password))
            teacher.User.UpdatePassword(request.Teacher.Password);

        if (request.Teacher.Courses is not null)
            teacher.UpdateCourses(request.Teacher.Courses.Adapt<List<Course>>());

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}