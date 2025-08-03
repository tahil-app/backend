using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    private readonly LocalizedStrings _localizedStrings;
    public CourseRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Course>())
    {
        _localizedStrings = localizedStrings;
    }

    public async Task<Result<bool>> AddCourseAsync(Course course, Guid tenantId)
    {
        var result = await CheckDuplicateCourseNameAsync(course);

        if (result.IsSuccess)
        {
            course.TenantId = tenantId;
            course.IsActive = true;
            Add(course);
        }

        return result;
    }

    public async Task<Result<bool>> DeleteCourseAsync(int id)
    {
        var course = await GetAsync(c => c.Id == id, [c => c.Schedules, c => c.Sessions, c => c.TeacherCourses]);

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

    private async Task<Result<bool>> CheckDuplicateCourseNameAsync(Course course) 
    {
        var existCourse = await GetAsync(u => u.Name == course.Name);

        // Check if course name is duplicated
        if (existCourse is not null && existCourse.Name == course.Name)
            return Result<bool>.Failure(_localizedStrings.DuplicatedCourse);

        return Result<bool>.Success(true);
    }
}