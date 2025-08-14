using Mapster;
using Tahil.Domain.Dtos;
using Z.EntityFramework.Plus;

namespace Tahil.Infrastructure.Repositories;

public class LookupRepository : Repository<ClassSchedule>, ILookupRepository
{
    private readonly BEContext _context;
    public LookupRepository(BEContext context) : base(context.Set<ClassSchedule>())
    {
        _context = context;
    }


    public async Task<Result<List<GroupDto>>> GetGroupsAsync(Guid tenantId)
    {
        var groups = await _context.Set<Group>().Where(r => r.TenantId == tenantId)
            .Select(r => new GroupDto { Id = r.Id, Name = r.Name }).ToListAsync();

        return Result<List<GroupDto>>.Success(groups);
    }

    public async Task<Result<List<RoomDto>>> GetRoomsAsync(Guid tenantId)
    {
        var rooms = await _context.Set<Room>().Where(r => r.TenantId == tenantId && r.IsActive)
            .Select(r => new RoomDto { Id = r.Id, Name = r.Name }).ToListAsync();

        return Result<List<RoomDto>>.Success(rooms);
    }

    public async Task<Result<List<CourseDto>>> GetCoursesAsync(Guid tenantId)
    {
        var courses = await _context.Set<Course>().Where(r => r.TenantId == tenantId && r.IsActive)
            .Select(r => new CourseDto { Id = r.Id, Name = r.Name }).ToListAsync();

        return Result<List<CourseDto>>.Success(courses);
    }

    public async Task<Result<List<TeacherDto>>> GetTeachersAsync(Guid tenantId)
    {
        var teachers = await _context.Set<Teacher>().Where(r => r.User.TenantId == tenantId && r.User.IsActive)
            .Select(r => new TeacherDto { Id = r.Id, Name = r.User.Name }).ToListAsync();

        return Result<List<TeacherDto>>.Success(teachers);
    }

    public async Task<Result<List<StudentDto>>> GetStudentsAsync(Guid tenantId)
    {
        var students = await _context.Set<Student>().Where(r => r.User.TenantId == tenantId && r.User.IsActive)
            .Select(r => new StudentDto { Id = r.Id, Name = r.User.Name }).ToListAsync();

        return Result<List<StudentDto>>.Success(students);
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

    public async Task<ClassSessionLookupsDto> GetClassSessionAsync(Guid tenantId, int courseId)
    {
        var roomsQuery = _context.Set<Room>().Where(r => r.IsActive).Select(r => new RoomDto { Id = r.Id, Name = r.Name }).Future();
        var teachersQuery = _context.Set<Teacher>().Where(r => r.TeacherCourses.Any(t => t.CourseId == courseId)).Select(r => new TeacherDto { Id = r.Id, Name = r.User.Name }).Future();

        var rooms = await roomsQuery.ToListAsync();

        var lookups = new ClassSessionLookupsDto
        {
            Rooms = rooms,
            Teachers = teachersQuery.ToList()
        };

        return lookups;
    }
}