using Tahil.Common.Exceptions;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    private readonly LocalizedStrings _localizedStrings;
    public CourseRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Course>())
    {
        _localizedStrings = localizedStrings;
    }

    public async Task AddCourseAsync(Course course)
    {
        await CheckExistsAsync(course);
        
        course.IsActive = true;
        Add(course);
    }

    private async Task CheckExistsAsync(Course course) 
    {
        var existCourse = await GetAsync(u => u.Name == course.Name);

        if (existCourse is not null && existCourse.Name == course.Name)
            throw new DomainException(_localizedStrings.DuplicatedCourse);
    }
}