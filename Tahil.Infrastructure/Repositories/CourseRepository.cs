using Tahil.Common.Exceptions;

namespace Tahil.Infrastructure.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(BEContext context) : base(context.Set<Course>())
    {
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
            throw new DomainException("The course exists, enter another.");
    }
}