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
            AuthorizationOperation.ViewDetail => await CanViewTeacherDetailAsync(context),
            AuthorizationOperation.ViewAll => CanViewAll(context),
            AuthorizationOperation.ViewPaged => CanViewPaged(context),
            AuthorizationOperation.Create => CanCreateTeacher(context),
            AuthorizationOperation.Update => await CanUpdateTeacherAsync(context),
            AuthorizationOperation.Delete => await CanDeleteTeacherAsync(context),
            AuthorizationOperation.Activate => await CanActivateOrDeActivateTeacherAsync(context),
            AuthorizationOperation.Deactivate => await CanActivateOrDeActivateTeacherAsync(context),
            _ => false
        };
    }

    private async Task<bool> CanViewTeacherDetailAsync(AuthorizationContext context)
    {
        var teacherExist = await teacherRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return teacherExist && context.HasAdminOrEmployeeAccess;
    }
    
    private bool CanViewAll(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }
    
    private bool CanViewPaged(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private bool CanCreateTeacher(AuthorizationContext context)
    {
        return context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanUpdateTeacherAsync(AuthorizationContext context)
    {
        if (context.MetaData == "attachment" && context.HasAdminOrEmployeeAccess)
        {
            return await attachmentRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        }

        if (context.MetaData == "course" && context.HasAdminOrEmployeeAccess)
        {
            return await courseRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        }

        var teacherExist = await teacherRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return teacherExist && context.HasAdminOrEmployeeAccess;
    }

    private async Task<bool> CanDeleteTeacherAsync(AuthorizationContext context)
    {
        var teacherExist = await teacherRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return teacherExist && context.IsAdmin;
    }

    private async Task<bool> CanActivateOrDeActivateTeacherAsync(AuthorizationContext context)
    {
        var teacherExist = await teacherRepository.ExistsInTenantAsync(context.EntityId, context.UserTenantId);
        return teacherExist && context.IsAdmin;
    }

}