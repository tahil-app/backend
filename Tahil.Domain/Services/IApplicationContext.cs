namespace Tahil.Domain.Services;

public interface IApplicationContext
{
    string UserName { get; }
    int UserId { get; }
    UserRole GetUserRole();
    bool IsAuthenticated();
}
