namespace Tahil.Domain.Authorization.Strategies;

public class ClassSessionAuthorizationStrategy(IClassSessionRepository classSessionRepository) 
    : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.ClassSession;

    public async Task<bool> CanAccessAsync(AuthorizationContext context)
    {
        return context.AuthorizationOperation switch
        {
            AuthorizationOperation.ViewAll => CanViewAll(context),
            AuthorizationOperation.ViewPaged => CanViewPaged(context),
            AuthorizationOperation.Create => CanCreate(context),
            //AuthorizationOperation.Update => await CanUpdateAsync(context),
            _ => false
        };
    }

    private bool CanViewAll(AuthorizationContext context)
    {
        return context.HasAnyAccess;
    }
    
    private bool CanViewPaged(AuthorizationContext context)
    {
        return context.HasAnyAccess;
    }

    private bool CanCreate(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    //private async Task<bool> CanUpdateAsync(AuthorizationContext context)
    //{
    //    var classSessionExist = await classSessionRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
    //    return classSessionExist && context.HasAdminOrEmployeeAccess;
    //}

} 