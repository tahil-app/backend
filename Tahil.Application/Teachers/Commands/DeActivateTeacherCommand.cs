namespace Tahil.Application.Teachers.Commands;

public record DeActivateTeacherCommand(int Id) : ICommand<Result<bool>>;

public class DeActivateTeacherCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository teacherRepository) : ICommandHandler<DeActivateTeacherCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeActivateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.Id);
        if (teacher is null)
            throw new NotFoundException("Teacher");

        teacher.DeActivate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}