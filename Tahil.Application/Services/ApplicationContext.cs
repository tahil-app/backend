using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tahil.Domain.Consts;
using Tahil.Domain.Entities;

namespace Tahil.Application.Services;

public class ApplicationContext : IApplicationContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    public ApplicationContext(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
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
            var id = claimuser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return id is not null ? int.Parse(id) : 0;
        }
    }

    public Guid TenantId
    {
        get
        {
            ClaimsPrincipal claimuser = _httpContextAccessor.HttpContext.User;
            var tenantId = claimuser.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;
            var result = Guid.TryParse(tenantId, out Guid convertedTenantId);
            return result ? convertedTenantId : Tenants.DarAlfor2an;
        }
    }

    public UserRole UserRole
    {
        get 
        {
            ClaimsPrincipal claimuser = _httpContextAccessor.HttpContext.User;
            var role = claimuser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return role is not null ? Enum.Parse<UserRole>(role) : UserRole.None;
        }
    }

    public async Task<User> GetUserAsync()
    {
        return (await _userRepository.GetAsync(r => r.Id == UserId))!;
    }

    public bool IsAuthenticated()
    {
        var userIdentity = _httpContextAccessor.HttpContext.User.Identity;
        return userIdentity is null ? false : userIdentity.IsAuthenticated;
    }

}
