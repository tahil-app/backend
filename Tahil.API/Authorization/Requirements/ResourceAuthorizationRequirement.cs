using Microsoft.AspNetCore.Authorization;
using Tahil.Domain.Enums;

namespace Tahil.API.Authorization.Requirements;

public class ResourceAuthorizationRequirement : IAuthorizationRequirement
{
    public EntityType EntityType { get; }
    public AuthorizationOperation Operation { get; }

    public ResourceAuthorizationRequirement(EntityType type, AuthorizationOperation operation)
    {
        EntityType = type;
        Operation = operation;
    }
} 