namespace Tahil.Application.Groups.Commands;

public record UpdateGroupStudentsCommand(int GroupId, List<int> StudentIds) : ICommand<Result<bool>>;

public class UpdateGroupStudentsCommandHandler(IUnitOfWork unitOfWork, IGroupRepository groupRepository, IApplicationContext applicationContext) : ICommandHandler<UpdateGroupStudentsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateGroupStudentsCommand request, CancellationToken cancellationToken)
    {
        var updateResult = await groupRepository.UpdateStudentsAsync(request.GroupId, request.StudentIds, applicationContext.TenantId);
        if (updateResult.IsSuccess)
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return updateResult;
    }
}