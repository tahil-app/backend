using Tahil.Common.Exceptions;

namespace Tahil.Infrastructure.Repositories;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    private readonly BEContext _context;
    public GroupRepository(BEContext context) : base(context.Set<Group>())
    {
        _context = context;
    }

    public async Task AddGroupAsync(Group group)
    {
        await CheckExistsAsync(group);

        Add(group);
    }

    public async Task DeleteGroupAsync(int groupId)
    {
        var hasStudents = await _context.Set<Student>().AnyAsync(r => r.StudentGroups.Select(g => g.GroupId == groupId).Any());
        if (hasStudents)
            throw new DomainException("This group has students, you can't delete it.");

        var group = await GetAsync(r => r.Id == groupId);
        HardDelete(group!);
    }

    private async Task CheckExistsAsync(Group group) 
    {
        var existGroup = await GetAsync(u => u.Name == group.Name);

        if (existGroup is not null && existGroup.Name == group.Name)
            throw new DuplicateException("Group");
    }
}