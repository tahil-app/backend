namespace Tahil.Domain.Authorization.Strategies;

public interface IEntityAuthorizationStrategy
{
    EntityType Type { get; }
    Task<bool> CanAccessAsync(AuthorizationContext context);
} 