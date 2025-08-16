namespace Tahil.Domain.Authorization;

public class AuthorizationContext
{
    private AuthorizationContext() { }

    public int? EntityId { get; private set; }
    public required EntityType EntityType { get; init; }
    public required AuthorizationOperation AuthorizationOperation { get; init; }
    public string? MetaData { get; init; }

    public Guid UserTenantId { get; init; }
    public bool IsAdmin { get; init; }
    public bool IsEmployee { get; init; }
    public bool IsTeacher { get; init; }
    public bool IsStudent { get; init; }

    public bool HasAdminOrEmployeeAccess => IsAdmin || IsEmployee;
    public bool HasAdminOrEmployeeOrTeacherAccess => IsAdmin || IsEmployee || IsTeacher;
    public bool HasAnyAccess => IsAdmin || IsEmployee || IsTeacher || IsStudent;

    public static AuthorizationContext Create(EntityType entityType, Guid userTenantId, UserRole userRole, AuthorizationOperation operation, int? entityId = null, string? metaData = null)
    {
        return new AuthorizationContext
        {
            EntityType = entityType,
            AuthorizationOperation = operation,
            EntityId = entityId,
            UserTenantId = userTenantId,
            MetaData = metaData,
            IsAdmin = userRole == UserRole.Admin,
            IsEmployee = userRole == UserRole.Employee,
            IsTeacher = userRole == UserRole.Teacher,
            IsStudent = userRole == UserRole.Student,
        };
    }
    
}