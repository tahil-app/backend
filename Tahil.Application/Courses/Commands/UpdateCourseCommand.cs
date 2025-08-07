using Tahil.Application.Courses.Validators;

namespace Tahil.Application.Courses.Commands;

public record UpdateCourseCommand(CourseDto Course) : ICourseCommand, ICommand<Result<bool>>;

public class UpdateCourseCommandHandler(IUnitOfWork unitOfWork, ICourseRepository courseRepository, LocalizedStrings locale, IApplicationContext applicationContext) : ICommandHandler<UpdateCourseCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(r => r.Id == request.Course.Id && r.TenantId == applicationContext.TenantId);
        if (course == null)
            return Result<bool>.Failure(locale.NotAvailableCourse);

        course.Update(request.Course);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}