namespace Tahil.Application.Courses.Commands;

public record DeleteCourseCommand(int Id) : ICommand<Result<bool>>;

public class DeleteCourseCommandHandler(IUnitOfWork unitOfWork, ICourseRepository courseRepository) : ICommandHandler<DeleteCourseCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await courseRepository.DeleteCourseAsync(request.Id);
        if (deleteResult.IsSuccess) 
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return deleteResult;
    }
}
