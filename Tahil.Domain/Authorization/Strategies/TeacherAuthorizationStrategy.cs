namespace Tahil.Domain.Authorization.Strategies;

public class TeacherAuthorizationStrategy(
    ITeacherRepository teacherRepository,
    IAttachmentRepository attachmentRepository,
    ICourseRepository courseRepository) 
    : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Teacher;

    public async Task<bool> CanAccessAsync(AuthorizationContext context)
    {
        return context.AuthorizationOperation switch
        {
            AuthorizationOperation.ViewDetail => await CanViewDetailAsync(context),
            AuthorizationOperation.ViewAll => CanViewAll(context),
            AuthorizationOperation.ViewPaged => CanViewPaged(context),
            AuthorizationOperation.Create => CanCreate(context),
            AuthorizationOperation.Update => await CanUpdateAsync(context),
            AuthorizationOperation.Delete => await CanDeleteAsync(context),
            AuthorizationOperation.Activate or
            AuthorizationOperation.DeActivate => await CanActivateOrDeActivateAsync(context),
            _ => false
        };
    }

    private async Task<bool> CanViewDetailAsync(AuthorizationContext context)
    {
        var teacherExist = await teacherRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return teacherExist && context.HasAdminOrEmployeeOrTeacherAccess;
    }
    
    private bool CanViewAll(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }
    
    private bool CanViewPaged(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private bool CanCreate(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanUpdateAsync(AuthorizationContext context)
    {
        if (context.MetaData == "attachment" && context.HasAdminOrEmployeeOrTeacherAccess)
        {
            return await attachmentRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        }

        if (context.MetaData == "course" && context.HasAdminOrEmployeeOrTeacherAccess)
        {
            return await courseRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        }

        var teacherExist = await teacherRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return teacherExist && context.HasAdminOrEmployeeOrTeacherAccess;
    }

    private async Task<bool> CanDeleteAsync(AuthorizationContext context)
    {
        if (context.MetaData == "attachment" && context.HasAdminOrEmployeeOrTeacherAccess)
        {
            return await attachmentRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        }

        var teacherExist = await teacherRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return teacherExist && context.IsAdmin;
    }

    private async Task<bool> CanActivateOrDeActivateAsync(AuthorizationContext context)
    {
        var teacherExist = await teacherRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return teacherExist && context.IsAdmin;
    }

}