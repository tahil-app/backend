namespace Tahil.Domain.Repositories;

public interface IGroupRepository : IRepository<Group> 
{
    Task<Result<bool>> AddGroupAsync(Group group, Guid tenantId);
    Task<Result<bool>> DeleteGroupAsync(int id, Guid tenantId);
    Task<Result<GroupDto>> GetGroupAsync(int id, Guid tenantId);
    Task<Result<bool>> UpdateStudentsAsync(int id, List<int> studentIds, Guid tenantId);
    Task<bool> ExistsInTenantAsync(int? id, Guid? tenantId);
}