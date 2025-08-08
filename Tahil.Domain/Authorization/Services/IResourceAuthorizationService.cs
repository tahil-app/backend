namespace Tahil.Domain.Authorization.Services;

public interface IResourceAuthorizationService
{
    Task<bool> CanAccessEntityAsync(AuthorizationContext authorizationContext);
} 