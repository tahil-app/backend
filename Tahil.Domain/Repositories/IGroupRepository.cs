namespace Tahil.Domain.Repositories;

public interface IGroupRepository : IRepository<Group> 
{
    Task AddGroupAsync(Group group);
    Task DeleteGroupAsync(int groupId);
}