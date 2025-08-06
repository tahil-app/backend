namespace Tahil.Domain.Dtos;

public class UserDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Gender Gender { get; set; }
    public DateOnly? JoinedDate { get; set; }
    public DateOnly? BirthDate { get; set; }
    public bool IsActive { get; set; }
    public string? ImagePath { get; set; }
}