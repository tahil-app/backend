namespace Tahil.Domain.Authorization.Services;

public interface IResourceAuthorizationService
{
    Task<bool> CanAccessEntityAsync(EntityType entityType, AuthorizationOperation operation, int? resourceId);
} 