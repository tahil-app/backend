namespace Tahil.Domain.Authorization.Strategies;

public class RoomAuthorizationStrategy(IRoomRepository roomRepository) : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Room;

    public async Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        return authorizationContext.AuthorizationOperation switch
        {
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
        var roomExist = await roomRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return roomExist && context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanDeleteAsync(AuthorizationContext context)
    {
        var roomExist = await roomRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return roomExist && context.IsAdmin;
    }

    private async Task<bool> CanActivateOrDeActivateAsync(AuthorizationContext context)
    {
        var roomExist = await roomRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return roomExist && context.IsAdmin;
    }

}