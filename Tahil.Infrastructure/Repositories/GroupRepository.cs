using Mapster;
using Tahil.Domain.Dtos;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    private readonly LocalizedStrings _localizedStrings;
    private readonly BEContext _context;
    
    public GroupRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Group>())
    {
        _localizedStrings = localizedStrings;
        _context = context;
    }

    public async Task<Result<bool>> AddGroupAsync(Group group, Guid tenantId)
    {
        var result = await CheckDuplicateGroupNameAsync(group, tenantId);

        if (result.IsSuccess)
        {
            group.TenantId = tenantId;
            Add(group);
        }

        return result;
    }

    public async Task<Result<bool>> DeleteGroupAsync(int id, Guid tenantId)
    {
        var group = await GetAsync(g => g.Id == id && g.TenantId == tenantId, [g => g.StudentGroups, g => g.Schedules]);

        if (group is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableGroup);

        // Check if group has child relationships
        if (group.StudentGroups.Any())
            return Result<bool>.Failure(_localizedStrings.GroupHasStudentCantDelete);

        if (group.Schedules.Any())
            return Result<bool>.Failure(_localizedStrings.GroupHasSchedules);

        // If no child relationships exist, proceed with deletion
        HardDelete(group);
        return Result<bool>.Success(true);
    }

    public async Task<Result<GroupDto>> GetGroupAsync(int id, Guid tenantId) 
    {
        var query = await _dbSet.Where(r => r.Id == id && r.TenantId == tenantId)
            .Select(r => new GroupDto
            {
                Id = r.Id,
                Name = r.Name,
                CourseId = r.CourseId,
                Course = r.Course.Adapt<CourseDto>(),
                TeacherId = r.TeacherId,
                Teacher = r.Teacher.Adapt<TeacherDto>(),
                Students = r.StudentGroups.Select(s => s.Student).ToList().Adapt<List<StudentDto>>(),
                Capacity = r.Capacity,
                NumberOfStudents = r.StudentGroups.Count
            }).FirstOrDefaultAsync();

        if (query == null)
            return Result<GroupDto>.Failure(_localizedStrings.NotAvailableGroup);

        return Result<GroupDto>.Success(query);
    }

    public async Task<Result<bool>> UpdateStudentsAsync(int id, List<int> studentIds, Guid tenantId)
    {
        // Get the group with its current students
        var group = await GetAsync(g => g.Id == id && g.TenantId == tenantId, [g => g.StudentGroups]);

        if (group is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableGroup);

        // Get the students to be added to the group
        var students = await _context.Set<Student>().Where(s => studentIds.Contains(s.Id)).ToListAsync();
        group.UpdateStudents(students);

        return Result<bool>.Success(true);
    }

    public async Task<bool> ExistsInTenantAsync(int id, Guid tenantId)
    {
        return await _dbSet.AnyAsync(g => g.Id == id && g.TenantId == tenantId);
    }

    private async Task<Result<bool>> CheckDuplicateGroupNameAsync(Group group, Guid tenantId) 
    {
        var existGroup = await _dbSet.AnyAsync(g => g.Name == group.Name && g.TenantId == tenantId);

        // Check if group name is duplicated
        if (existGroup)
            return Result<bool>.Failure(_localizedStrings.DuplicatedGroup);

        return Result<bool>.Success(true);
    }

}