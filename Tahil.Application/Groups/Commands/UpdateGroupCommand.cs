namespace Tahil.Application.Groups.Commands;

public record UpdateGroupCommand(GroupDto GroupDto) : ICommand<Result<bool>>;

public class UpdateGroupCommandHandler(IUnitOfWork unitOfWork, IGroupRepository groupRepository) : ICommandHandler<UpdateGroupCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await groupRepository.GetAsync(r => r.Id == request.GroupDto.Id);
        if (group == null)
            throw new NotFoundException("Group");

        group.Update(request.GroupDto);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}