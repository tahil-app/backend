using Tahil.API.Authorization.Requirements;
using Tahil.Domain.Enums;

namespace Tahil.API.Authorization;

public static class AuthorizationExtensions
{
    public static RouteHandlerBuilder RequireAccess(this RouteHandlerBuilder builder, EntityType entityType, AuthorizationOperation authorizationOperation)
    {
        return builder.RequireAuthorization(policy => 
            policy.AddRequirements(new ResourceAuthorizationRequirement(entityType, authorizationOperation)));
    }
}