namespace Tahil.Application.Courses.Commands;

public record DeActivateCourseCommand(int Id) : ICommand<Result<bool>>;

public class DeActivateCourseCommandHandler(IUnitOfWork unitOfWork, ICourseRepository courseRepository) : ICommandHandler<DeActivateCourseCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeActivateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(r => r.Id == request.Id);
        if (course is null)
            throw new NotFoundException("Course");

        course.DeActivate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(result);
    }
}