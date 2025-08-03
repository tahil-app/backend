using Tahil.Application.Courses.Mappings;
using Tahil.Application.Courses.Validators;

namespace Tahil.Application.Courses.Commands;

public record CreateCourseCommand(CourseDto Course) : ICourseCommand, ICommand<Result<bool>>;

public class CreateCourseCommandHandler(IUnitOfWork unitOfWork, ICourseRepository courseRepository, IApplicationContext applicationContext) : ICommandHandler<CreateCourseCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var addResult = await courseRepository.AddCourseAsync(request.Course.ToCourse(), applicationContext.TenantId);

        if (addResult.IsSuccess)
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return addResult;
    }
}