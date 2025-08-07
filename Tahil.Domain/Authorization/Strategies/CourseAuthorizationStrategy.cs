using Tahil.Domain.Repositories;

namespace Tahil.Domain.Authorization.Strategies;

public class CourseAuthorizationStrategy(ICourseRepository courseRepository) : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Course;

    public async Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        var canAccess = false;
        switch (authorizationContext.AuthorizationOperation)
        {
            case AuthorizationOperation.ViewDetail:
                canAccess = await CanViewCourseDetail(authorizationContext);
                break;

            case AuthorizationOperation.ViewAll:
                canAccess = CanViewAll(authorizationContext);
                break;

            case AuthorizationOperation.ViewPaged:
                canAccess = CanViewPaged(authorizationContext);
                break;

            case AuthorizationOperation.Create:
                canAccess = CanCreateCourse(authorizationContext);
                break;

            case AuthorizationOperation.UpdateWithId:
            case AuthorizationOperation.UpdateWithEntity:
                canAccess = await CanUpdateCourse(authorizationContext);
                break;

            case AuthorizationOperation.Delete:
                canAccess = await CanDeleteCourse(authorizationContext);
                break;

            case AuthorizationOperation.Activate:
            case AuthorizationOperation.DeActivate:
                canAccess = await CanActivateOrDeActivateCourse(authorizationContext);
                break;
        }

        return canAccess;
    }

    private async Task<bool> CanViewCourseDetail(AuthorizationContext authorizationContext)
    {
        // Admin and Employee can access any course
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return true;

        // Teacher can only access courses they are assigned to
        if (authorizationContext.IsTeacher)
        {
            var course = await courseRepository.GetAsync(g =>
            g.Id == authorizationContext.EntityId &&
            g.TenantId == authorizationContext.UserTenantId);
            return course != null;
        }

        return false;
    }

    private bool CanViewAll(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee || authorizationContext.IsTeacher)
            return true;

        return false;
    }

    private bool CanViewPaged(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return true;

        return false;
    }

    private bool CanCreateCourse(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return true;

        return false;
    }

    private async Task<bool> CanUpdateCourse(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return true;

        var course = await courseRepository.GetAsync(g => g.Id == authorizationContext.EntityId && g.TenantId == authorizationContext.UserTenantId);
        return course != null;
    }

    private async Task<bool> CanDeleteCourse(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin)
            return true;

        var course = await courseRepository.GetAsync(g => g.Id == authorizationContext.EntityId && g.TenantId == authorizationContext.UserTenantId);
        return course != null;
    }

    private async Task<bool> CanActivateOrDeActivateCourse(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin)
            return true;

        var course = await courseRepository.GetAsync(g => g.Id == authorizationContext.EntityId && g.TenantId == authorizationContext.UserTenantId);
        return course != null;
    }

}