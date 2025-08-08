namespace Tahil.Domain.Authorization.Strategies;

public class CourseAuthorizationStrategy(ICourseRepository courseRepository) : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Course;

    public async Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        return authorizationContext.AuthorizationOperation switch
        {
            AuthorizationOperation.ViewDetail => await CanViewCourseDetailAsync(authorizationContext),

            AuthorizationOperation.ViewAll => CanViewAll(authorizationContext),

            AuthorizationOperation.ViewPaged => CanViewPaged(authorizationContext),

            AuthorizationOperation.Create => CanCreateCourse(authorizationContext),

            AuthorizationOperation.UpdateWithId or 
            AuthorizationOperation.UpdateWithEntity => await CanUpdateCourseAsync(authorizationContext),

            AuthorizationOperation.Delete => await CanDeleteCourseAsync(authorizationContext),

            AuthorizationOperation.Activate or AuthorizationOperation.DeActivate => await CanActivateOrDeActivateCourseAsync(authorizationContext),

            _ => false
        };
    }

    private async Task<bool> CanViewCourseDetailAsync(AuthorizationContext context)
    {
        var courseExist = await courseRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return courseExist && context.HasAdminOrEmployeeOrTeacherAccess;
    }

    private static bool CanViewAll(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeOrTeacherAccess;
    }

    private static bool CanViewPaged(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private static bool CanCreateCourse(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanUpdateCourseAsync(AuthorizationContext context)
    {
        var courseExist = await courseRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return courseExist && context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanDeleteCourseAsync(AuthorizationContext context)
    {
        var courseExist = await courseRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return courseExist && context.IsAdmin;
    }

    private async Task<bool> CanActivateOrDeActivateCourseAsync(AuthorizationContext context)
    {
        var courseExist = await courseRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return courseExist && context.IsAdmin;
    }

}