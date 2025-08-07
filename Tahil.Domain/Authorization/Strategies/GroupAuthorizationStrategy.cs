namespace Tahil.Domain.Authorization.Strategies;

public class GroupAuthorizationStrategy(IGroupRepository groupRepository) : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Group;

    public async Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        var canAccess = false;
        switch (authorizationContext.AuthorizationOperation)
        {
            case AuthorizationOperation.ViewDetail:
                canAccess = await CanViewGroupDetail(authorizationContext);
                break;

            case AuthorizationOperation.ViewAll:
                canAccess = CanViewAll(authorizationContext);
                break;

            case AuthorizationOperation.ViewPaged:
                canAccess = CanViewPaged(authorizationContext);
                break;

            case AuthorizationOperation.Create:
                canAccess = CanCreateGroup(authorizationContext);
                break;

            case AuthorizationOperation.UpdateWithId:
            case AuthorizationOperation.UpdateWithEntity:
                canAccess = await CanUpdateGroup(authorizationContext);
                break;

            case AuthorizationOperation.Delete:
                canAccess = await CanDeleteGroup(authorizationContext);
                break;
        }

        return canAccess;
    }

    private async Task<bool> CanViewGroupDetail(AuthorizationContext authorizationContext)
    {
        // Admin and Employee can access any group
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return true;

        // Teacher can only access groups they are assigned to
        if (authorizationContext.IsTeacher)
        {
            var group = await groupRepository.GetAsync(g =>
            g.Id == authorizationContext.EntityId &&
            g.TeacherId == authorizationContext.UserId &&
            g.TenantId == authorizationContext.UserTenantId);
            return group != null;
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
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee || authorizationContext.IsTeacher)
            return true;

        return false;
    }

    private bool CanCreateGroup(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return true;

        return false;
    }

    private async Task<bool> CanUpdateGroup(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return true;

        var group = await groupRepository.GetAsync(g => g.Id == authorizationContext.EntityId && g.TenantId == authorizationContext.UserTenantId);
        return group != null;
    }

    private async Task<bool> CanDeleteGroup(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin)
            return true;

        var group = await groupRepository.GetAsync(g => g.Id == authorizationContext.EntityId && g.TenantId == authorizationContext.UserTenantId);
        return group != null;
    }
}