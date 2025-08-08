namespace Tahil.Domain.Authorization.Strategies;

public class GroupAuthorizationStrategy(IGroupRepository groupRepository) : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Group;

    public async Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        return authorizationContext.AuthorizationOperation switch
        {
            AuthorizationOperation.ViewDetail => await CanViewDetailAsync(authorizationContext),
            AuthorizationOperation.ViewAll => CanViewAll(authorizationContext),
            AuthorizationOperation.ViewPaged => CanViewPaged(authorizationContext),
            AuthorizationOperation.Create => CanCreateGroup(authorizationContext),
            AuthorizationOperation.Update => await CanUpdateAsync(authorizationContext),
            AuthorizationOperation.Delete => await CanDeleteAsync(authorizationContext),
            _ => false
        };
    }

    private async Task<bool> CanViewDetailAsync(AuthorizationContext context)
    {
        var groupExist = await groupRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return groupExist && context.HasAdminOrEmployeeOrTeacherAccess;
    }

    private static bool CanViewAll(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private static bool CanViewPaged(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeOrTeacherAccess;
    }

    private static bool CanCreateGroup(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanUpdateAsync(AuthorizationContext context)
    {
        var groupExist = await groupRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return groupExist && context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanDeleteAsync(AuthorizationContext context)
    {
        var groupExist = await groupRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return groupExist && context.IsAdmin;
    }

}