namespace Tahil.Domain.Authorization.Strategies;

public class StudentAttendanceAuthorizationStrategy(IClassSessionRepository classSessionRepository) 
    : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.StudentAttendance;

    public async Task<bool> CanAccessAsync(AuthorizationContext context)
    {
        return context.AuthorizationOperation switch
        {
            AuthorizationOperation.ViewAll => CanViewAll(context),
            AuthorizationOperation.Update => await CanUpdateAsync(context),
            _ => false
        };
    }

    private bool CanViewAll(AuthorizationContext context)
    {
        return context.HasAnyAccess;
    }
    
    private async Task<bool> CanUpdateAsync(AuthorizationContext context)
    {
        var classSessionExist = await classSessionRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return classSessionExist && context.HasAdminOrEmployeeOrTeacherAccess;
    }

} 