using Tahil.Application.Courses.Mappings;

namespace Tahil.Application.Courses.Commands;

public record CreateCourseCommand(CourseDto CourseDto) : ICommand<Result<bool>>;

public class CreateCourseCommandHandler(IUnitOfWork unitOfWork, ICourseRepository courseRepository) : ICommandHandler<CreateCourseCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        await courseRepository.AddCourseAsync(request.CourseDto.ToCourse());

        var result = await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(result);
    }
}