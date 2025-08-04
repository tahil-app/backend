using Tahil.Application.Models;

namespace Tahil.Application.Teachers.Commands;

public record UpdateTeacherPasswordCommand(UpdatePasswordParams Params) : ICommand<Result<bool>>;

public class UpdateTeacherPasswordCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository teacherRepository, LocalizedStrings locale) : ICommandHandler<UpdateTeacherPasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateTeacherPasswordCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.Params.Id || r.User.Email.Value == request.Params.Email);
        if (teacher is null)
            return Result<bool>.Failure(locale.NotAvailableTeacher);

        teacher.User.UpdatePassword(request.Params.Password);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}