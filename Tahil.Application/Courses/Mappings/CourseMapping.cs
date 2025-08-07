using Tahil.Domain.Entities;

namespace Tahil.Application.Courses.Mappings;

public static class CourseMapping
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Course, CourseDto>.NewConfig()
            .Map(dest => dest.NumberOfTeachers, src => src.TeacherCourses.Count);
    }

    public static Course ToCourse(this CourseDto model)
    {
        return new Course
        {
            Name = model.Name,
            Description = model.Description,
            IsActive = model.IsActive,
        };
    }
}