using Tahil.Application.Models;

namespace Tahil.Application.Students.Commands;

public record UpdateStudentPasswordCommand(UpdatePasswordParams Params) : ICommand<Result<bool>>;

public class UpdateStudentPasswordCommandHandler(IUnitOfWork unitOfWork, IStudentRepository studentRepository) : ICommandHandler<UpdateStudentPasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateStudentPasswordCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetAsync(r => r.Id == request.Params.Id || r.User.Email.Value == request.Params.Email);
        if (student is null)
            throw new NotFoundException("Student");

        student.User.UpdatePassword(request.Params.Password);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}