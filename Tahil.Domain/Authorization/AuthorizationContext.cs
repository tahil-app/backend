namespace Tahil.Domain.Authorization;

public class AuthorizationContext
{
    private AuthorizationContext() { }

    public int? EntityId { get; private set; }
    public required EntityType EntityType { get; init; }
    public required AuthorizationOperation AuthorizationOperation { get; init; }
    public required User User { get; init; }
    public string? MetaData { get; init; }

    public Guid? UserTenantId => User.TenantId;
    public bool IsAdmin => User.Role == UserRole.Admin;
    public bool IsEmployee => User.Role == UserRole.Employee;
    public bool IsTeacher => User.Role == UserRole.Teacher;
    public bool IsStudent => User.Role == UserRole.Student;

    public bool HasAdminOrEmployeeAccess => IsAdmin || IsEmployee;
    public bool HasAdminOrEmployeeOrTeacherAccess => IsAdmin || IsEmployee || IsTeacher;

    public static AuthorizationContext Create(EntityType entityType, User user, AuthorizationOperation operation, int? entityId = null, string? metaData = null)
    {
        return new AuthorizationContext
        {
            EntityType = entityType,
            AuthorizationOperation = operation,
            EntityId = entityId,
            User = user,
            MetaData = metaData
        };
    }
    
}