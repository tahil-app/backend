using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;
using Z.EntityFramework.Plus;

namespace Tahil.Infrastructure.Repositories;

public class LookupRepository : Repository<ClassSchedule>, ILookupRepository
{
    private readonly BEContext _context;
    public LookupRepository(BEContext context) : base(context.Set<ClassSchedule>())
    {
        _context = context;
    }


    public async Task<Result<List<LookupDto>>> GetGroupsAsync(Guid tenantId)
    {
        var groups = await _context.Set<Group>().Where(r => r.TenantId == tenantId)
            .Select(r => new LookupDto { Id = r.Id, Name = r.Name }).ToListAsync();

        return Result<List<LookupDto>>.Success(groups);
    }

    public async Task<Result<List<LookupDto>>> GetRoomsAsync(Guid tenantId)
    {
        var rooms = await _context.Set<Room>().Where(r => r.TenantId == tenantId && r.IsActive)
            .Select(r => new LookupDto { Id = r.Id, Name = r.Name }).ToListAsync();

        return Result<List<LookupDto>>.Success(rooms);
    }

    public async Task<Result<List<LookupDto>>> GetCoursesAsync(Guid tenantId)
    {
        var courses = await _context.Set<Course>().Where(r => r.TenantId == tenantId && r.IsActive)
            .Select(r => new LookupDto { Id = r.Id, Name = r.Name }).ToListAsync();

        return Result<List<LookupDto>>.Success(courses);
    }

    public async Task<Result<List<LookupDto>>> GetTeachersAsync(Guid tenantId)
    {
        var teachers = await _context.Set<Teacher>().Where(r => r.User.TenantId == tenantId && r.User.IsActive)
            .Select(r => new LookupDto { Id = r.Id, Name = r.User.Name }).ToListAsync();

        return Result<List<LookupDto>>.Success(teachers);
    }

    public async Task<Result<List<LookupDto>>> GetStudentsAsync(Guid tenantId)
    {
        var students = await _context.Set<Student>().Where(r => r.User.TenantId == tenantId && r.User.IsActive)
            .Select(r => new LookupDto { Id = r.Id, Name = r.User.Name }).ToListAsync();

        return Result<List<LookupDto>>.Success(students);
    }

    public async Task<ClassScheduleLookupsDto> GetClassScheduleAsync(Guid tenantId)
    {
        var roomsQuery = _context.Set<Room>().Where(r => r.IsActive).Select(r => new RoomDto { Id = r.Id, Name = r.Name }).Future();
        var groupsQuery = _context.Set<Group>().Select(r => new GroupDto { Id = r.Id, Name = r.Name }).Future();

        var rooms = await roomsQuery.ToListAsync();

        var lookups = new ClassScheduleLookupsDto
        {
            Rooms = rooms,
            Groups = groupsQuery.ToList()
        };

        return lookups;
    }

    public async Task<ClassSessionLookupsDto> GetClassSessionAsync(Guid tenantId, int courseId,int userId, UserRole userRole)
    {
        var roomsQuery = _context.Set<Room>().Where(r => r.IsActive && r.TenantId == tenantId).AsQueryable();
        var teachersQuery = _context.Set<Teacher>().Where(r => r.User.TenantId == tenantId && r.User.IsActive && r.TeacherCourses.Any(t => t.CourseId == courseId)).AsQueryable();
        var coursesQuery = _context.Set<Course>().Where(c => c.IsActive).AsQueryable();
        var groupsQuery = _context.Set<Group>().AsQueryable();

        if (userRole == UserRole.Teacher)
        {
            roomsQuery = roomsQuery.Where(r => r.Sessions.Any(s => s.TeacherId == userId));
            coursesQuery = coursesQuery.Where(r => r.TeacherCourses.Any(t => t.TeacherId == userId));
            groupsQuery = groupsQuery.Where(r => r.TeacherId == userId);
        }
        else if(userRole == UserRole.Student)
        {
            roomsQuery = roomsQuery.Where(r => r.Sessions.Any(s => s.Schedule!.Group!.StudentGroups.Any(s => s.StudentId == userId)));
            coursesQuery = coursesQuery.Where(r => r.Groups.Any(t => t.StudentGroups.Any(s => s.StudentId == userId)));
            groupsQuery = groupsQuery.Where(r => r.StudentGroups.Any(s => s.StudentId == userId));
        }

        var roomsFuture = roomsQuery.Select(r => new RoomDto { Id = r.Id, Name = r.Name }).Future();
        var teachersFuture = courseId != 0 ? teachersQuery.Select(r => new TeacherDto { Id = r.Id, Name = r.User.Name }).Future() : null;
        var coursesFuture = courseId == 0 ? coursesQuery.Select(r => new CourseDto { Id = r.Id, Name = r.Name }).Future() : null;
        var groupsFuture = courseId == 0 ? groupsQuery.Select(r => new GroupDto { Id = r.Id, Name = r.Name }).Future() : null;

        var rooms = await roomsFuture.ToListAsync();

        var lookups = new ClassSessionLookupsDto
        {
            Rooms = rooms,
            Teachers = teachersFuture is not null ? teachersFuture.ToList() : new(),
            Courses = coursesFuture is not null ? coursesFuture.ToList() : new(),
            Groups = groupsFuture is not null ? groupsFuture.ToList() : new(),
        };

        return lookups;
    }
}