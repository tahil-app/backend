using Tahil.Common.Exceptions;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly LocalizedStrings locale;
    public UserRepository(BEContext context, LocalizedStrings localized) : base(context.Set<User>())
    {
        locale = localized;
    }

    public async Task AddUserAsync(User newUser)
    {
        newUser.IsActive = true;

        var existUser = await GetAsync(u => u.Email.Value == newUser.Email.Value || u.PhoneNumber == newUser.PhoneNumber);

        if (existUser is not null && existUser.Email.Value == newUser.Email.Value)
            throw new DomainException(locale.DuplicatedEmail);

        if (existUser is not null && existUser.PhoneNumber == newUser.PhoneNumber)
            throw new DomainException(locale.DuplicatedPhoneNumber);

        newUser.UpdatePassword(newUser.Password);

        Add(newUser);
    }
}