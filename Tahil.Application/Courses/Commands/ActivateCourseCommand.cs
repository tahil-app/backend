namespace Tahil.Application.Courses.Commands;

public record ActivateCourseCommand(int Id) : ICommand<Result<bool>>;

public class ActivateCourseCommandHandler(IUnitOfWork unitOfWork, ICourseRepository courseRepository, LocalizedStrings locale) : ICommandHandler<ActivateCourseCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ActivateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(r => r.Id == request.Id);
        if (course is null)
            return Result<bool>.Failure(locale.NotAvailableCourse);

        course.Activate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}