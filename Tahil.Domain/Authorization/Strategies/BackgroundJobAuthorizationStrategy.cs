namespace Tahil.Domain.Authorization.Strategies;

public class BackgroundJobAuthorizationStrategy() : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.BackgroundJob;

    public async Task<bool> CanAccessAsync(AuthorizationContext authorizationContext)
    {
        return authorizationContext.AuthorizationOperation switch
        {
            AuthorizationOperation.Update => CanUpdate(authorizationContext),

            _ => false
        };
    }

    private static bool CanUpdate(AuthorizationContext context)
    {
        return context.IsAdmin;
    }

}