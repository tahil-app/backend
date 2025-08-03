using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Tahil.Application.Services;

public class ApplicationContext : IApplicationContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ApplicationContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserName
    {
        get 
        {
            ClaimsPrincipal claimuser = _httpContextAccessor.HttpContext.User;
            var name = claimuser.Claims.FirstOrDefault(c => c.Type == "Name")?.Value;
            return name ?? "Anonymous";
        }
    }

    public int UserId
    {
        get
        {
            ClaimsPrincipal claimuser = _httpContextAccessor.HttpContext.User;
            var id = claimuser.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            return id is not null ? int.Parse(id) : 0;
        }
    }

    public Guid TenantId
    {
        get
        {
            ClaimsPrincipal claimuser = _httpContextAccessor.HttpContext.User;
            var id = claimuser.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;
            return id is not null ? Guid.Parse(id) : new Guid("0");
        }
    }

    public UserRole GetUserRole()
    {
        ClaimsPrincipal claimuser = _httpContextAccessor.HttpContext.User;
        var role = claimuser.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        return role is not null ? Enum.Parse<UserRole>(role) : UserRole.None;
    }

    public bool IsAuthenticated()
    {
        var userIdentity = _httpContextAccessor.HttpContext.User.Identity;
        return userIdentity is null ? false : userIdentity.IsAuthenticated;
    }

}
