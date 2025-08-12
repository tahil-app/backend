namespace Tahil.Domain.Authorization.Strategies;

public class ClassScheduleAuthorizationStrategy(IClassScheduleRepository classScheduleRepository) 
    : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.ClassSchedule;

    public async Task<bool> CanAccessAsync(AuthorizationContext context)
    {
        return context.AuthorizationOperation switch
        {
            AuthorizationOperation.ViewAll => CanViewAll(context),
            AuthorizationOperation.ViewPaged => CanViewPaged(context),
            AuthorizationOperation.Create => CanCreate(context),
            AuthorizationOperation.Update => await CanUpdateAsync(context),
            AuthorizationOperation.Delete => await CanDeleteAsync(context),
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

    private async Task<bool> CanUpdateAsync(AuthorizationContext context)
    {
        var classScheduleExist = await classScheduleRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return classScheduleExist && context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanDeleteAsync(AuthorizationContext context)
    {
        var classScheduleExist = await classScheduleRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return classScheduleExist && context.IsAdmin;
    }

} 