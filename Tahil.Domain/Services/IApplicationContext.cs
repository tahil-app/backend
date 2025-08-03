namespace Tahil.Domain.Services;

public interface IApplicationContext
{
    string UserName { get; }
    int UserId { get; }
    Guid TenantId { get; }
    UserRole GetUserRole();
    bool IsAuthenticated();
}
