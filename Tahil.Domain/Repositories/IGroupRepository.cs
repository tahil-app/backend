namespace Tahil.Domain.Repositories;

public interface IGroupRepository : IRepository<Group> 
{
    Task<Result<bool>> AddGroupAsync(Group group, Guid tenantId);
    Task<Result<bool>> DeleteGroupAsync(int id, Guid tenantId);
    Task<GroupDto> GetGroupAsync(int id, Guid tenantId);
}