namespace Tahil.Application.Teachers.Commands;

public record ActivateTeacherCommand(int Id) : ICommand<Result<bool>>;

public class ActivateTeacherCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository teacherRepository) : ICommandHandler<ActivateTeacherCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ActivateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.Id);
        if (teacher is null)
            throw new NotFoundException("Teacher");

        teacher.Activate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(result);
    }
}