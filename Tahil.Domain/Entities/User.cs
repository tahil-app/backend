using Tahil.Common.Helpers;

namespace Tahil.Domain.Entities;

public class User: Base
{
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Email Email { get; set; } = default!;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Gender Gender { get; set; }
    public DateOnly JoinedDate { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool IsActive { get; set; }
    public string? ImagePath { get; set; }

    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    public void Validate() 
    {
        Check.IsNull(Name, nameof(Name));
        Check.IsNull(Email, nameof(Email));
        Check.IsNull(PhoneNumber, nameof(PhoneNumber));
        Check.IsNull(Role, nameof(Role));
    }

    public void Activate() => IsActive = true;
    public void DeActivate() => IsActive = false;

    public void SetRole(UserRole role) => Role = role;
    
    public void Update(UserDto userDto) 
    {
        Name = userDto.Name;
        PhoneNumber = userDto.PhoneNumber;
        Email = Email.Create(userDto.Email);
        Gender = userDto.Gender;
        JoinedDate = userDto.JoinedDate;
        BirthDate = userDto.BirthDate;

        Validate();
    }

    public void ChangeImage(string imagePath) 
    {
        ImagePath = imagePath;
    }

    public void UpdatePassword(string password) 
    {
        Password = PasswordHasher.Hash(password);
    }
}