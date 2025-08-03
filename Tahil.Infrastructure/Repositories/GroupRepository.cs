using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    private readonly LocalizedStrings _localizedStrings;
    public GroupRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Group>())
    {
        _localizedStrings = localizedStrings;
    }

    public async Task<Result<bool>> AddGroupAsync(Group group, Guid tenantId)
    {
        var result = await CheckDuplicateGroupNameAsync(group);

        if (result.IsSuccess)
        {
            group.TenantId = tenantId;
            Add(group);
        }

        return result;
    }

    public async Task<Result<bool>> DeleteGroupAsync(int id)
    {
        var group = await GetAsync(g => g.Id == id, [g => g.StudentGroups, g => g.Schedules, g => g.Sessions]);

        if (group is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableGroup);

        // Check if group has child relationships
        if (group.StudentGroups.Any())
            return Result<bool>.Failure(_localizedStrings.GroupHasStudentCantDelete);

        if (group.Schedules.Any())
            return Result<bool>.Failure(_localizedStrings.GroupHasSchedules);

        if (group.Sessions.Any())
            return Result<bool>.Failure(_localizedStrings.GroupHasSessions);

        // If no child relationships exist, proceed with deletion
        HardDelete(group);
        return Result<bool>.Success(true);
    }

    private async Task<Result<bool>> CheckDuplicateGroupNameAsync(Group group) 
    {
        var existGroup = await GetAsync(g => g.Name == group.Name);

        // Check if group name is duplicated
        if (existGroup is not null && existGroup.Name == group.Name)
            return Result<bool>.Failure(_localizedStrings.DuplicatedGroup);

        return Result<bool>.Success(true);
    }
}