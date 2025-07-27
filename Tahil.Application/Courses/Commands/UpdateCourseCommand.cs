namespace Tahil.Application.Courses.Commands;

public record UpdateCourseCommand(CourseDto CourseDto) : ICommand<Result<bool>>;

public class UpdateCourseCommandHandler(IUnitOfWork unitOfWork, ICourseRepository courseRepository) : ICommandHandler<UpdateCourseCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(r => r.Id == request.CourseDto.Id);
        if (course == null) 
            throw new NotFoundException("Course");

        course.Update(request.CourseDto);

        var result = await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}