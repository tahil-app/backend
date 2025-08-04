using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly LocalizedStrings _localizedStrings;
    public UserRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<User>())
    {
        _localizedStrings = localizedStrings;
    }

    public async Task<Result<bool>> AddUserAsync(User user, Guid tenantId)
    {
        var result = await CheckDuplicateUserAsync(user);

        if (result.IsSuccess)
        {
            user.TenantId = tenantId;
            user.IsActive = true;
            user.UpdatePassword(user.Password);
            Add(user);
        }

        return result;
    }

    public async Task<Result<bool>> DeleteUserAsync(int id)
    {
        var user = await GetAsync(u => u.Id == id);

        if (user is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableUser);

        // Check if user has child relationships
        if (user.Teachers.Any())
            return Result<bool>.Failure(_localizedStrings.UserHasTeachers);

        if (user.Students.Any())
            return Result<bool>.Failure(_localizedStrings.UserHasStudents);

        // If no child relationships exist, proceed with deletion
        HardDelete(user);
        return Result<bool>.Success(true);
    }

    private async Task<Result<bool>> CheckDuplicateUserAsync(User user) 
    {
        var existUser = await GetAsync(u => u.Email.Value == user.Email.Value || u.PhoneNumber == user.PhoneNumber);

        // Check if email is duplicated
        if (existUser is not null && existUser.Email.Value == user.Email.Value)
            return Result<bool>.Failure(_localizedStrings.DuplicatedEmail);

        // Check if phone number is duplicated
        if (existUser is not null && existUser.PhoneNumber == user.PhoneNumber)
            return Result<bool>.Failure(_localizedStrings.DuplicatedPhoneNumber);
        
        return Result<bool>.Success(true);
    }
}