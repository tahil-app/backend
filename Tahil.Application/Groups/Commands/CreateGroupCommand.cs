using Tahil.Application.Groups.Mappings;

namespace Tahil.Application.Groups.Commands;

public record CreateGroupCommand(GroupDto GroupDto) : ICommand<Result<bool>>;

public class CreateGroupCommandHandler(IUnitOfWork unitOfWork, IGroupRepository groupRepository) : ICommandHandler<CreateGroupCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        await groupRepository.AddGroupAsync(request.GroupDto.ToGroup());

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}