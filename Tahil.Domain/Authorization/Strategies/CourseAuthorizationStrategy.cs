namespace Tahil.Domain.Authorization.Strategies;

public class CourseAuthorizationStrategy(ICourseRepository courseRepository) : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Course;

    public async Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        return authorizationContext.AuthorizationOperation switch
        {
            AuthorizationOperation.ViewDetail => await CanViewDetailAsync(authorizationContext),
            AuthorizationOperation.ViewAll => CanViewAll(authorizationContext),
            AuthorizationOperation.ViewPaged => CanViewPaged(authorizationContext),
            AuthorizationOperation.Create => CanCreate(authorizationContext),
            AuthorizationOperation.Update => await CanUpdateAsync(authorizationContext),
            AuthorizationOperation.Delete => await CanDeleteAsync(authorizationContext),
            AuthorizationOperation.Activate or 
            AuthorizationOperation.DeActivate => await CanActivateOrDeActivateAsync(authorizationContext),
            _ => false
        };
    }

    private async Task<bool> CanViewDetailAsync(AuthorizationContext context)
    {
        var courseExist = await courseRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return courseExist && context.HasAdminOrEmployeeOrTeacherAccess;
    }

    private static bool CanViewAll(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private static bool CanViewPaged(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private static bool CanCreate(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanUpdateAsync(AuthorizationContext context)
    {
        var courseExist = await courseRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return courseExist && context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanDeleteAsync(AuthorizationContext context)
    {
        var courseExist = await courseRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return courseExist && context.IsAdmin;
    }

    private async Task<bool> CanActivateOrDeActivateAsync(AuthorizationContext context)
    {
        var courseExist = await courseRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return courseExist && context.IsAdmin;
    }

}