namespace Tahil.Domain.Services;

public interface IApplicationContext
{
    string UserName { get; }
    int UserId { get; }
    Guid TenantId { get; }
    UserRole UserRole { get; }
    bool IsAuthenticated();
    Task<User> GetUserAsync();
}
