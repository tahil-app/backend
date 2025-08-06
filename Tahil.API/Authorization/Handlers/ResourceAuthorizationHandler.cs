using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Tahil.API.Authorization.Requirements;
using Tahil.Domain.Authorization.Services;
using Tahil.Domain.Enums;

namespace Tahil.API.Authorization.Handlers;

public class ResourceAuthorizationHandler(
    IResourceAuthorizationService resourceAuthorizationService,
    IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<ResourceAuthorizationRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceAuthorizationRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            context.Fail();
            return;
        }

        // Get user information from claims
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        var roleClaim = context.User.FindFirst(ClaimTypes.Role);

        if (userIdClaim == null || roleClaim == null)
        {
            context.Fail();
            return;
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            context.Fail();
            return;
        }

        // Extract resource ID from route
        var resourceId = ExtractResourceIdFromRoute(httpContext);
        if (resourceId == null && requirement.Operation.RequireId())
        {
            context.Fail();
            return;
        }

        // Check if user can access the resource
        var canAccess = await resourceAuthorizationService.CanAccessEntityAsync(requirement.EntityType, requirement.Operation, resourceId);

        if (canAccess)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    private int? ExtractResourceIdFromRoute(HttpContext httpContext)
    {
        // Try to get the ID from route values
        if (httpContext.Request.RouteValues.TryGetValue("id", out var idValue))
        {
            if (int.TryParse(idValue?.ToString(), out var id))
                return id;
        }

        // Try to get from query string
        if (httpContext.Request.Query.TryGetValue("id", out var queryId))
        {
            if (int.TryParse(queryId.ToString(), out var id))
                return id;
        }

        return null;
    }
}