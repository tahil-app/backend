namespace Tahil.Domain.Enums;

public enum AuthorizationOperation
{
    ViewDetail,
    ViewAll,
    ViewPaged,
    Create,
    Update,
    Delete,
}

public static class AuthorizationOperationHelper 
{
    public static bool RequireId(this AuthorizationOperation operation) => operation switch
    {
        AuthorizationOperation.ViewDetail => true,
        AuthorizationOperation.Delete => true,
        _ => false
    };
}