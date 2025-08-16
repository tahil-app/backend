namespace Tahil.Domain.Authorization.Strategies;

public class StudentAttendanceAuthorizationStrategy() 
    : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.StudentAttendance;

    public async Task<bool> CanAccessAsync(AuthorizationContext context)
    {
        return context.AuthorizationOperation switch
        {
            AuthorizationOperation.ViewAll => CanViewAll(context),
            AuthorizationOperation.Update => CanUpdate(context),
            _ => false
        };
    }

    private bool CanViewAll(AuthorizationContext context)
    {
        return context.HasAnyAccess;
    }
    
    private bool CanUpdate(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeOrTeacherAccess;
    }

} 