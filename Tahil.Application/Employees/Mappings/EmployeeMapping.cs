using Tahil.Domain.Entities;
using Tahil.Domain.ValueObjects;

namespace Tahil.Application.Employees.Mappings;


public static class EmployeeMapping
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Student, StudentDto>.NewConfig()
            .Map(dest => dest.Name, src => src.User.Name)
            .Map(dest => dest.Email, src => src.User.Email.Value)
            .Map(dest => dest.PhoneNumber, src => src.User.PhoneNumber)
            .Map(dest => dest.Role, src => src.User.Role)
            .Map(dest => dest.Gender, src => src.User.Gender)
            .Map(dest => dest.JoinedDate, src => src.User.JoinedDate)
            .Map(dest => dest.BirthDate, src => src.User.BirthDate)
            .Map(dest => dest.IsActive, src => src.User.IsActive);
    }

    public static User ToUser(this StudentDto model)
    {
        return new User
        {
            Name = model.Name,
            Email = Email.Create(model.Email),
            PhoneNumber = model.PhoneNumber,
            Password = model.Password,
            Role = model.Role,
            Gender = model.Gender,
            JoinedDate = model.JoinedDate,
            BirthDate = model.BirthDate,
            IsActive = model.IsActive,
        };
    }
}