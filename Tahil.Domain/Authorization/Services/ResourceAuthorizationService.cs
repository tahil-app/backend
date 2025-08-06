using Tahil.Domain.Authorization.Strategies;

namespace Tahil.Domain.Authorization.Services;

public class ResourceAuthorizationService(IEnumerable<IEntityAuthorizationStrategy> strategies, IApplicationContext applicationContext) 
    : IResourceAuthorizationService
{

    public async Task<bool> CanAccessEntityAsync(EntityType entityType, AuthorizationOperation operation, int? resourceId)
    {

        var strategy = strategies.FirstOrDefault(s => s.Type == entityType);

        if (strategy == null)
            return false;

        var user = await applicationContext.GetUserAsync();

        return await strategy.CanAccessAsync(AuthorizationContext.CreateFunctionalityOverID(entityType, user, operation, resourceId));
    }
}