namespace Tahil.Application.Courses.Commands;

public record DeActivateCourseCommand(int Id) : ICommand<Result<bool>>;

public class DeActivateCourseCommandHandler(IUnitOfWork unitOfWork, ICourseRepository courseRepository, LocalizedStrings locale, IApplicationContext applicationContext) : ICommandHandler<DeActivateCourseCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeActivateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(r => r.Id == request.Id && r.TenantId == applicationContext.TenantId);
        if (course is null)
            return Result<bool>.Failure(locale.NotAvailableCourse);

        course.DeActivate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}