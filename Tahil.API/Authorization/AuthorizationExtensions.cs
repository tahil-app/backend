using Tahil.API.Authorization.Requirements;
using Tahil.Domain.Enums;

namespace Tahil.API.Authorization;

public static class AuthorizationExtensions
{
    public static RouteHandlerBuilder RequireAccess(
        this RouteHandlerBuilder builder, 
        EntityType type, 
        AuthorizationOperation operation, 
        string? property = null,
        string? metaData = null)
    {
        return builder.RequireAuthorization(policy =>
            policy.AddRequirements(new ResourceAuthorizationRequirement(type, operation, property, metaData)));
    }

}