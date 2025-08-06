namespace Tahil.Domain.Authorization.Strategies;

public class CourseAuthorizationStrategy(ITeacherRepository teacherRepository) : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Course;

    public async Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        // Admin and Employee can access any course
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return true;

        // Teacher can only access courses they are assigned to
        if (authorizationContext.IsTeacher)
        {
            var teacher = await teacherRepository.GetAsync(t => t.Id == authorizationContext.UserId, includes: [r => r.TeacherCourses.Any(c => c.CourseId == authorizationContext.EntityId)]);
            return teacher != null;
        }

        return false;
    }
}