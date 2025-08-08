using Microsoft.AspNetCore.Authorization;
using Tahil.Domain.Enums;

namespace Tahil.API.Authorization.Requirements;

public class ResourceAuthorizationRequirement : IAuthorizationRequirement
{
    public EntityType EntityType { get; }
    public AuthorizationOperation Operation { get; }
    public string? Property { get; set; }
    public string? MetaDate { get; set; }

    public ResourceAuthorizationRequirement(EntityType type, AuthorizationOperation operation, string? property = null, string? metaDate = null)
    {
        EntityType = type;
        Operation = operation;
        Property = property;
        MetaDate = metaDate;
    }
}