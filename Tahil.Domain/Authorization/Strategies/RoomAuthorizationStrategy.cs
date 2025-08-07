namespace Tahil.Domain.Authorization.Strategies;

public class RoomAuthorizationStrategy(IRoomRepository roomRepository) : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Room;

    public async Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        var canAccess = false;
        switch (authorizationContext.AuthorizationOperation)
        {
            case AuthorizationOperation.ViewAll:
                canAccess = CanViewAll(authorizationContext);
                break;

            case AuthorizationOperation.ViewPaged:
                canAccess = CanViewPaged(authorizationContext);
                break;

            case AuthorizationOperation.Create:
                canAccess = CanCreateRoom(authorizationContext);
                break;

            case AuthorizationOperation.UpdateWithEntity:
                canAccess = await CanUpdateRoom(authorizationContext);
                break;

            case AuthorizationOperation.Delete:
                canAccess = await CanDeleteRoom(authorizationContext);
                break;
            
            case AuthorizationOperation.Activate:
            case AuthorizationOperation.DeActivate:
                canAccess = await CanActivateOrDeActivateRoom(authorizationContext);
                break;
        }

        return canAccess;
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

    private bool CanCreateRoom(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return true;

        return false;
    }

    private async Task<bool> CanUpdateRoom(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return true;

        var room = await roomRepository.GetAsync(g => g.Id == authorizationContext.EntityId && g.TenantId == authorizationContext.UserTenantId);
        return room != null;
    }

    private async Task<bool> CanDeleteRoom(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin)
            return true;

        var room = await roomRepository.GetAsync(g => g.Id == authorizationContext.EntityId && g.TenantId == authorizationContext.UserTenantId);
        return room != null;
    }

    private async Task<bool> CanActivateOrDeActivateRoom(AuthorizationContext authorizationContext)
    {
        if (authorizationContext.IsAdmin)
            return true;

        var room = await roomRepository.GetAsync(g => g.Id == authorizationContext.EntityId && g.TenantId == authorizationContext.UserTenantId);
        return room != null;
    }

}