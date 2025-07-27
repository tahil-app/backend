using Tahil.Application.Auth.Models;
using Tahil.Domain.Entities;
using Tahil.Domain.ValueObjects;

namespace Tahil.Application.Auth.Mappings;

public static class SignupMapping
{
    public static void RegisterMappings()
    {

    }

    public static User ToUser(this SignupModel model) 
    {
        return new User 
        {
            Email = Email.Create(model.Email),
            Name = model.Name,
            Password = model.Password,
            PhoneNumber = model.PhoneNumber,
            Role = UserRole.Admin
        };
    }
}