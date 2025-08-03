using Tahil.Application.Groups.Validators;

namespace Tahil.Application.Groups.Commands;

public record UpdateGroupCommand(GroupDto Group) : IGroupCommand, ICommand<Result<bool>>;

public class UpdateGroupCommandHandler(IUnitOfWork unitOfWork, IGroupRepository groupRepository, LocalizedStrings locale) : ICommandHandler<UpdateGroupCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await groupRepository.GetAsync(r => r.Id == request.Group.Id);
        if (group == null)
            return Result<bool>.Failure(locale.NotAvailableGroup);

        group.Update(request.Group);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}