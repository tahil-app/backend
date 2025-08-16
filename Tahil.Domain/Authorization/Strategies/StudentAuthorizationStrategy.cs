namespace Tahil.Domain.Authorization.Strategies;

public class StudentAuthorizationStrategy(
    IStudentRepository studentRepository,
    IAttachmentRepository attachmentRepository,
    IGroupRepository groupRepository) 
    : IEntityAuthorizationStrategy
{
    public EntityType Type => EntityType.Student;

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
        var studentExist = await studentRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return studentExist && context.HasAnyAccess;
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

        if (context.MetaData == "group" && context.HasAdminOrEmployeeOrTeacherAccess)
        {
            return await groupRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        }

        var studentExist = await studentRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return studentExist && context.HasAnyAccess;
    }

    private async Task<bool> CanDeleteAsync(AuthorizationContext context)
    {
        if (context.MetaData == "attachment" && (context.HasAdminOrEmployeeAccess || context.IsStudent))
        {
            return await attachmentRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        }

        var studentExist = await studentRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return studentExist && context.IsAdmin;
    }

    private async Task<bool> CanActivateOrDeActivateAsync(AuthorizationContext context)
    {
        var studentExist = await studentRepository.ExistsInTenantAsync(context.EntityId.GetValueOrDefault(), context.UserTenantId);
        return studentExist && context.IsAdmin;
    }

} 