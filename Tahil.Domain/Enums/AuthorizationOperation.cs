namespace Tahil.Domain.Enums;

public enum AuthorizationOperation
{
    ViewDetail,
    ViewAll,
    ViewPaged,
    Create,
    UpdateWithEntity,
    UpdateWithId,
    Delete,
    Activate,
    DeActivate
}

public static class AuthorizationOperationHelper 
{
    public static bool RequireId(this AuthorizationOperation operation) => operation switch
    {
        AuthorizationOperation.ViewDetail => true,
        AuthorizationOperation.UpdateWithId => true,
        AuthorizationOperation.Delete => true,
        AuthorizationOperation.Activate => true,
        AuthorizationOperation.DeActivate => true,
        _ => false
    };
}