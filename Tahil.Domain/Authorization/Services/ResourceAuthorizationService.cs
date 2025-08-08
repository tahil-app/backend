using Tahil.Domain.Authorization.Strategies;

namespace Tahil.Domain.Authorization.Services;

public class ResourceAuthorizationService(IEnumerable<IEntityAuthorizationStrategy> strategies) 
    : IResourceAuthorizationService
{

    public async Task<bool> CanAccessEntityAsync(AuthorizationContext authorizationContext)
    {
        var strategy = strategies.FirstOrDefault(s => s.Type == authorizationContext.EntityType);

        if (strategy == null)
            return false;

        return await strategy.CanAccessAsync(authorizationContext);
    }
}