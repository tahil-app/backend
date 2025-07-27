using Tahil.Domain.Entities;
using Tahil.Domain.ValueObjects;

namespace Tahil.Application.Teachers.Mappings;

public static class TeacherMapping
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Teacher, TeacherDto>.NewConfig()
            .Map(dest => dest.Name, src => src.User.Name)
            .Map(dest => dest.Email, src => src.User.Email.Value)
            .Map(dest => dest.PhoneNumber, src => src.User.PhoneNumber)
            .Map(dest => dest.Role, src => src.User.Role)
            .Map(dest => dest.Gender, src => src.User.Gender)
            .Map(dest => dest.JoinedDate, src => src.User.JoinedDate)
            .Map(dest => dest.BirthDate, src => src.User.BirthDate)
            .Map(dest => dest.IsActive, src => src.User.IsActive);
    }

    public static Teacher ToTeacher(this TeacherDto model)
    {
        var user = new User 
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

        return new Teacher
        {
            User = user,
            Experience = model.Experience,
            Qualification = model.Qualification,
        };
    }
}