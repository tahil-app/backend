using Mapster;
using Tahil.Domain.Dtos;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    private readonly BEContext _context;
    private readonly LocalizedStrings _localizedStrings;
    public CourseRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Course>())
    {
        _context = context;
        _localizedStrings = localizedStrings;
    }

    public async Task<Result<bool>> AddCourseAsync(Course course, Guid tenantId)
    {
        var result = await CheckDuplicateCourseNameAsync(course, tenantId);

        if (result.IsSuccess)
        {
            course.TenantId = tenantId;
            course.IsActive = true;
            Add(course);
        }

        return result;
    }

    public async Task<Result<bool>> DeleteCourseAsync(int id, Guid tenantId)
    {
        var course = await GetAsync(c => c.Id == id && c.TenantId == tenantId, [c => c.Schedules, c => c.Sessions, c => c.TeacherCourses]);

        if (course is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableCourse);

        // Check if course has child relationships
        if (course.Schedules.Any())
            return Result<bool>.Failure(_localizedStrings.CourseHasSchedules);

        if (course.Sessions.Any())
            return Result<bool>.Failure(_localizedStrings.CourseHasSessions);

        if (course.TeacherCourses.Any())
            return Result<bool>.Failure(_localizedStrings.CourseHasTeachers);

        // If no child relationships exist, proceed with deletion
        HardDelete(course);
        return Result<bool>.Success(true);
    }

    public async Task<Result<CourseDto>> GetCourseAsync(int id, Guid tenantId)
    {
        var course = await _dbSet.Where(r => r.Id == id && r.TenantId == tenantId)
            .Select(r => new CourseDto 
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsActive = r.IsActive,
                NumberOfTeachers = r.TeacherCourses.Count,
                Teachers = r.TeacherCourses.Where(r => r.Teacher.User.IsActive).Select(r => new TeacherDto { Id = r.TeacherId, Name = r.Teacher.User.Name }).ToList()
            }).FirstOrDefaultAsync();

        if (course is null)
            return Result<CourseDto>.Failure(_localizedStrings.NotAvailableCourse);

        return Result<CourseDto>.Success(course!);
    }

    public async Task<Result<bool>> UpdateTeachersAsync(int id, List<int> teacherIds, Guid tenantId)
    {
        // Get the course with its current teachers
        var course = await GetAsync(g => g.Id == id && g.TenantId == tenantId, [g => g.TeacherCourses]);

        if (course is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableCourse);

        // Get the teachers to be added to the group
        var teachers = await _context.Set<Teacher>().Where(s => teacherIds.Contains(s.Id)).ToListAsync();
        course.UpdateTeachers(teachers);

        return Result<bool>.Success(true);
    }

    private async Task<Result<bool>> CheckDuplicateCourseNameAsync(Course course, Guid tenantId) 
    {
        var existCourse = await GetAsync(u => u.Name == course.Name && u.TenantId == tenantId);

        // Check if course name is duplicated
        if (existCourse is not null && existCourse.Name == course.Name)
            return Result<bool>.Failure(_localizedStrings.DuplicatedCourse);

        return Result<bool>.Success(true);
    }
}