namespace Tahil.Application.Courses.Commands;

public record ActivateCourseCommand(int Id) : ICommand<Result<bool>>;

public class ActivateCourseCommandHandler(IUnitOfWork unitOfWork, ICourseRepository courseRepository) : ICommandHandler<ActivateCourseCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ActivateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(r => r.Id == request.Id);
        if (course is null)
            throw new NotFoundException("Course");

        course.Activate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}