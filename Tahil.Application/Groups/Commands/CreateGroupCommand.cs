using Tahil.Application.Groups.Mappings;
using Tahil.Application.Groups.Validators;

namespace Tahil.Application.Groups.Commands;

public record CreateGroupCommand(GroupDto Group) : IGroupCommand, ICommand<Result<bool>>;

public class CreateGroupCommandHandler(IUnitOfWork unitOfWork, IGroupRepository groupRepository, IApplicationContext applicationContext) : ICommandHandler<CreateGroupCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var addResult = await groupRepository.AddGroupAsync(request.Group.ToGroup(), applicationContext.TenantId);

        if (addResult.IsSuccess)
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(result);
        }

        return addResult;
    }
}