namespace Tahil.Application.Courses.Commands;

public record UpdateCourseTeachersCommand(int CourseId, List<int> TeacherIds) : ICommand<Result<bool>>;

public class UpdateCourseTeachersCommandHandler(IUnitOfWork unitOfWork, ICourseRepository courseRepository, IApplicationContext applicationContext) : ICommandHandler<UpdateCourseTeachersCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateCourseTeachersCommand request, CancellationToken cancellationToken)
    {
        var updateResult = await courseRepository.UpdateTeachersAsync(request.CourseId, request.TeacherIds, applicationContext.TenantId);
        if (updateResult.IsSuccess)
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(updateResult.IsSuccess ? true : result);
        }

        return updateResult;
    }
}