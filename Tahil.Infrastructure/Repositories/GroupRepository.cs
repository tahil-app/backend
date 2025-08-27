using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;
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
        // Check if group exists and has any child relationships in a single query
        var groupWithRelationships = await _dbSet
            .Where(g => g.Id == id && g.TenantId == tenantId)
            .Select(g => new
            {
                Group = g,
                HasStudents = g.StudentGroups.Any(),
                HasSchedules = g.Schedules.Any()
            })
            .FirstOrDefaultAsync();

        if (groupWithRelationships == null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableGroup);

        // Check if group has child relationships
        if (groupWithRelationships.HasStudents)
            return Result<bool>.Failure(_localizedStrings.GroupHasStudentCantDelete);

        if (groupWithRelationships.HasSchedules)
            return Result<bool>.Failure(_localizedStrings.GroupHasSchedules);

        // If no child relationships exist, proceed with deletion
        HardDelete(groupWithRelationships.Group);
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
                Course = new LookupDto { Id = r.Id, Name = r.Course.Name },
                TeacherId = r.TeacherId,
                Teacher = new LookupDto { Id = r.TeacherId, Name = r.Teacher.User.Name },
                Students = r.StudentGroups.Select(s => new LookupDto { Id = s.Student.Id, Name = s.Student.User.Name }).ToList(),
                Capacity = r.Capacity,
                NumberOfStudents = r.StudentGroups.Count
            }).FirstOrDefaultAsync();

        if (query == null)
            return Result<GroupDto>.Failure(_localizedStrings.NotAvailableGroup);

        return Result<GroupDto>.Success(query);
    }

    public async Task<List<DailyScheduleDto>> GetGroupSchedulesAsync(int id, Guid tenantId)
    {
        var result = await _dbSet.Where(r => r.Id == id && r.TenantId == tenantId)
            .SelectMany(g => g.Schedules)
            .Select(s => new DailyScheduleDto
            {
                Day = s.Day,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                RoomName = s.Room!.Name,
                GroupName = s.Group!.Name,
                CourseName = s.Group!.Course!.Name,
            })
            .OrderBy(s => s.StartTime)
            .ToListAsync();

        return result.OrderBy(r => r.Day).ToList();
    }

    public async Task<Result<List<GroupDailyAttendance>>> GetGroupAttendancesAsync(int id, int year, Guid tenantId)
    {
        var query = await _dbSet.Where(r => r.Id == id && r.Schedules.Any(s => s.Sessions.Any(ss => ss.Date.Year == year)) && r.TenantId == tenantId)
            .SelectMany(s => s.Schedules.SelectMany(r => r.Sessions)).GroupBy(s => s.Date).Select(a => new GroupDailyAttendance
            {
                Date = a.Key,
                SessionId = a.FirstOrDefault().Id,
                Absent = a.Where(r => r.StudentAttendances.Any(sa => sa.Status == AttendanceStatus.Absent)).Count(),
                Late = a.Where(r => r.StudentAttendances.Any(sa => sa.Status == AttendanceStatus.Late)).Count(),
                Present = a.Where(r => r.StudentAttendances.Any(sa => sa.Status == AttendanceStatus.Present)).Count(),
            }).ToListAsync();

        return Result<List<GroupDailyAttendance>>.Success(query.OrderByDescending(s => s.Date).ToList());
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