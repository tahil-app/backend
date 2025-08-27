namespace Tahil.Domain.Authorization.Strategies;

public class StudentAttendanceAuthorizationStrategy(IClassSessionRepository classSessionRepository, IApplicationContext applicationContext) 
    : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.StudentAttendance;

    public async Task<bool> CanAccessAsync(AuthorizationContext context)
    {
        return context.AuthorizationOperation switch
        {
            AuthorizationOperation.ViewAll => CanViewAll(context),
            AuthorizationOperation.ViewDetail => await CanViewDetailAsync(context),
            AuthorizationOperation.Update => await CanUpdateAsync(context),
            _ => false
        };
    }

    private async Task<bool> CanViewDetailAsync(AuthorizationContext context)
    {
        if (applicationContext.UserRole == UserRole.Teacher) 
        {
            var session = await classSessionRepository.GetAsync(r => 
                r.Id == context.EntityId.GetValueOrDefault() && 
                r.TeacherId == applicationContext.UserId &&
                r.TenantId == context.UserTenantId);

            return session != null && context.HasAdminOrEmployeeOrTeacherAccess;
        }


        var sessionExist = await classSessionRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return sessionExist && context.HasAdminOrEmployeeAccess;
    }

    private bool CanViewAll(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeOrTeacherAccess;
    }
    
    private async Task<bool> CanUpdateAsync(AuthorizationContext context)
    {
        var classSessionExist = await classSessionRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return classSessionExist && context.HasAdminOrEmployeeOrTeacherAccess;
    }

} 