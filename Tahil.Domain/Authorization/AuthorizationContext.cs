namespace Tahil.Domain.Authorization;

public class AuthorizationContext
{
    private AuthorizationContext() { }

    public int? EntityId { get; set; }
    public BaseDto? DTO { get; set; }

    public required EntityType EntityType { get; set; }
    public required AuthorizationOperation AuthorizationOperation { get; set; }
    public required User User { get; set; }

    public int UserId => User.Id;
    public bool IsAdmin => User.Role == UserRole.Admin;
    public bool IsEmployee => User.Role == UserRole.Employee;
    public bool IsTeacher => User.Role == UserRole.Teacher;
    public bool IsStudent => User.Role == UserRole.Student;


    public static AuthorizationContext CreateFunctionalityOverID(EntityType entityType, User user, AuthorizationOperation operation, int? id)
    {
        if (id == 0)
        {
            throw new ArgumentException("Id must be greater than 0");
        }

        return new AuthorizationContext()
        {
            EntityType = entityType,
            AuthorizationOperation = operation,
            EntityId = id,
            User = user
        };
    }
}