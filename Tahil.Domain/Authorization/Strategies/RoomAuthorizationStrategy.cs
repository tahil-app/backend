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

            AuthorizationOperation.Create => CanCreateRoom(authorizationContext),

            AuthorizationOperation.UpdateWithEntity => await CanUpdateRoomAsync(authorizationContext),

            AuthorizationOperation.Delete => await CanDeleteRoomAsync(authorizationContext),

            AuthorizationOperation.Activate or 
            AuthorizationOperation.DeActivate => await CanActivateOrDeActivateRoomAsync(authorizationContext),

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

    private static bool CanCreateRoom(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanUpdateRoomAsync(AuthorizationContext context)
    {
        var roomExist = await roomRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return roomExist && context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanDeleteRoomAsync(AuthorizationContext context)
    {
        var roomExist = await roomRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return roomExist && context.IsAdmin;
    }

    private async Task<bool> CanActivateOrDeActivateRoomAsync(AuthorizationContext context)
    {
        var roomExist = await roomRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return roomExist && context.IsAdmin;
    }

}