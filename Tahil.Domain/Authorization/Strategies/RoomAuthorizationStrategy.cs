namespace Tahil.Domain.Authorization.Strategies;

public class RoomAuthorizationStrategy : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Room;

    public Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        // Admin and Employee can access any room
        if (authorizationContext.IsAdmin || authorizationContext.IsEmployee)
            return Task.FromResult(true);

        return Task.FromResult(false);
    }
} 