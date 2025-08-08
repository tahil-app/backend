namespace Tahil.Domain.Authorization.Strategies;

public class GroupAuthorizationStrategy(IGroupRepository groupRepository) : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Group;

    public async Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        return authorizationContext.AuthorizationOperation switch
        {
            AuthorizationOperation.ViewDetail => await CanViewGroupDetailAsync(authorizationContext),

            AuthorizationOperation.ViewAll => CanViewAll(authorizationContext),

            AuthorizationOperation.ViewPaged => CanViewPaged(authorizationContext),

            AuthorizationOperation.Create => CanCreateGroup(authorizationContext),

            AuthorizationOperation.UpdateWithId or 
            AuthorizationOperation.UpdateWithEntity => await CanUpdateGroupAsync(authorizationContext),

            AuthorizationOperation.Delete => await CanDeleteGroupAsync(authorizationContext),
            _ => false
        };
    }

    private async Task<bool> CanViewGroupDetailAsync(AuthorizationContext context)
    {
        var groupExist = await groupRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return groupExist && context.HasAdminOrEmployeeAccess;
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

    private async Task<bool> CanUpdateGroupAsync(AuthorizationContext context)
    {
        var groupExist = await groupRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return groupExist && context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanDeleteGroupAsync(AuthorizationContext context)
    {
        var groupExist = await groupRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return groupExist && context.IsAdmin;
    }

}