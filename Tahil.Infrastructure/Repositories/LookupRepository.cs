using Mapster;
using Tahil.Domain.Dtos;
using Z.EntityFramework.Plus;

namespace Tahil.Infrastructure.Repositories;

public class LookupRepository : Repository<LessonSchedule>, ILookupRepository
{
    private readonly BEContext _context;
    public LookupRepository(BEContext context) : base(context.Set<LessonSchedule>())
    {
        _context = context;
    }

    public async Task<LessonScheduleLookupsDto> GetLessonScheduleLookupsAsync()
    {
        var roomsQuery = _context.Set<Room>().Where(r => r.IsActive).Future();
        var coursesQuery = _context.Set<Course>().Where(r => r.IsActive).Future();
        var groupsQuery = _context.Set<Group>().Future();
        var teachersQuery = _context.Set<Teacher>().Include(r => r.TeacherCourses).Where(r => r.User.IsActive && r.TeacherCourses.Any()).Future();

        var courses = await coursesQuery.ToListAsync();
        var teachers = teachersQuery.ToList();
        var courseTeachers = new List<CourseTeachers>();

        courses.ForEach(crs => 
        {
            courseTeachers.Add(new CourseTeachers
            { 
                Course = crs.Adapt<CourseDto>(), 
                Teachers = teachers.Where(r => r.TeacherCourses.Any(t => t.CourseId == crs.Id)).Adapt<List<TeacherDto>>()
            });
        });


        var lookups = new LessonScheduleLookupsDto
        {
            Rooms = roomsQuery.ToList().Adapt<List<RoomDto>>(),
            Groups = groupsQuery.ToList().Adapt<List<GroupDto>>(),
            CourseTeachers = courseTeachers
        };

        return lookups;
    }
}