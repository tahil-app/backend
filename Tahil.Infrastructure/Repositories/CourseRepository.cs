using Tahil.Domain.Dtos;
using Tahil.Domain.Entities;
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
        // Check if course exists and has any child relationships in a single query
        var courseWithRelationships = await _dbSet
            .Where(r => r.Id == id && r.TenantId == tenantId)
            .Select(r => new
            {
                Course = r,
                HasGroups = r.Groups.Any(),
                HasTeacherCourses = r.TeacherCourses.Any()
            })
            .FirstOrDefaultAsync();

        if (courseWithRelationships is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableCourse);

        // Check if course has child relationships
        if (courseWithRelationships.HasGroups)
            return Result<bool>.Failure(_localizedStrings.CourseHasGroups);

        if (courseWithRelationships.HasTeacherCourses)
            return Result<bool>.Failure(_localizedStrings.CourseHasTeachers);

        // If no child relationships exist, proceed with deletion
        HardDelete(courseWithRelationships.Course);
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
                Teachers = r.TeacherCourses.Where(r => r.Teacher.User.IsActive).Select(r => new LookupDto { Id = r.TeacherId, Name = r.Teacher.User.Name }).ToList(),
                Groups = r.Groups.Select(r => new LookupDto { Id = r.Id, Name = r.Name }).ToList()
            }).FirstOrDefaultAsync();

        if (course is null)
            return Result<CourseDto>.Failure(_localizedStrings.NotAvailableCourse);

        return Result<CourseDto>.Success(course!);
    }

    public async Task<Result<bool>> UpdateTeachersAsync(int id, List<int> teacherIds, Guid tenantId)
    {
        // Get the course with its current teachers
        var course = await GetAsync(g => g.Id == id && g.TenantId == tenantId, [g => g.TeacherCourses, g => g.Groups]);

        if (course is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableCourse);

       
        var groupTeacherIds = course.Groups.Select(g => g.TeacherId).Distinct().ToList();
        var missingTeachers = groupTeacherIds.Except(teacherIds).ToList();
        if (missingTeachers.Any())
            return Result<bool>.Failure(_localizedStrings.TeacherAndCourseHasGroup);


        // Get the teachers to be added to the group
        var teachers = await _context.Set<Teacher>().Where(s => teacherIds.Contains(s.Id)).ToListAsync();
        course.UpdateTeachers(teachers);

        return Result<bool>.Success(true);
    }

    public async Task<bool> ExistsInTenantAsync(int id, Guid tenantId)
    {
        return await _dbSet.AnyAsync(c => c.Id == id && c.TenantId == tenantId);
    }

    private async Task<Result<bool>> CheckDuplicateCourseNameAsync(Course course, Guid tenantId)
    {
        var existCourse = await _dbSet.AnyAsync(u => u.Name == course.Name && u.TenantId == tenantId);

        // Check if course name is duplicated
        if (existCourse)
            return Result<bool>.Failure(_localizedStrings.DuplicatedCourse);

        return Result<bool>.Success(true);
    }
}