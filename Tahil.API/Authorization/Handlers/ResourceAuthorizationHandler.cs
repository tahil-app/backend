using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;
using Tahil.API.Authorization.Requirements;
using Tahil.Domain.Authorization;
using Tahil.Domain.Authorization.Services;

namespace Tahil.API.Authorization.Handlers;

public class ResourceAuthorizationHandler(
    IResourceAuthorizationService resourceAuthorizationService,
    IHttpContextAccessor httpContextAccessor,
    IApplicationContext applicationContext)
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

        // Extract resource ID if property is specified
        var resourceId = await ExtractResourceId(httpContext, requirement);

        // Create authorization context
        var authorizationContext = AuthorizationContext.Create(requirement.EntityType, applicationContext.TenantId, applicationContext.UserRole, requirement.Operation, resourceId, requirement.MetaDate);

        // Check if user can access the resource
        var canAccess = await resourceAuthorizationService.CanAccessEntityAsync(authorizationContext);

        if (canAccess)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    private async Task<int?> ExtractResourceId(HttpContext httpContext, ResourceAuthorizationRequirement requirement)
    {
        var property = requirement.Property ?? "id";

        // Try route first
        var resourceId = ExtractFromRoute(httpContext, property);
        if (resourceId != null) return resourceId;

        // Try query string
        resourceId = ExtractFromQuery(httpContext, property);
        if (resourceId != null) return resourceId;

        // Try body
        resourceId = await ExtractFromBodyAsync(httpContext, property);
        if (resourceId != null) return resourceId;

        return null;
    }

    private int? ExtractFromRoute(HttpContext httpContext, string property)
    {
        if (httpContext.Request.RouteValues.TryGetValue(property, out var value))
        {
            if (int.TryParse(value?.ToString(), out var id))
                return id;
        }
        return null;
    }

    private int? ExtractFromQuery(HttpContext httpContext, string property)
    {
        if (httpContext.Request.Query.TryGetValue(property, out var value))
        {
            if (int.TryParse(value.ToString(), out var id))
                return id;
        }
        return null;
    }

    public async Task<int?> ExtractFromBodyAsync(HttpContext context, string property)
    {
        try
        {
            var contentType = context.Request.ContentType?.ToLowerInvariant();

            var possibleNames = new[] { property, "id", "Id", "ID", "entityId", "EntityId" }.Distinct();

            // 1️⃣ Handle JSON body
            if (contentType != null && contentType.Contains("application/json"))
            {
                context.Request.EnableBuffering();

                if (context.Request.Body.CanSeek)
                    context.Request.Body.Position = 0;

                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();

                context.Request.Body.Position = 0;

                if (!string.IsNullOrWhiteSpace(body))
                {
                    using var doc = JsonDocument.Parse(body);
                    var root = doc.RootElement;

                    foreach (var name in possibleNames)
                    {
                        if (root.TryGetProperty(name, out var element))
                        {
                            if (element.ValueKind == JsonValueKind.Number && element.TryGetInt32(out var num))
                                return num;

                            if (element.ValueKind == JsonValueKind.String && int.TryParse(element.GetString(), out num))
                                return num;
                        }
                    }
                }
            }

            // 2️⃣ Handle Form body (x-www-form-urlencoded or multipart/form-data)
            if (contentType != null && (contentType.Contains("application/x-www-form-urlencoded") || contentType.Contains("multipart/form-data")))
            {
                // Ensure the form is read
                if (!context.Request.HasFormContentType)
                    return null;

                var form = await context.Request.ReadFormAsync();

                foreach (var name in possibleNames)
                {
                    if (form.TryGetValue(name, out var value))
                    {
                        if (int.TryParse(value.ToString(), out var num))
                            return num;
                    }
                }
            }
        }
        catch
        {
            // Log error or ignore silently
        }

        return null;

    }

}