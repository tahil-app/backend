using Tahil.Application.Auth.Models;
using Tahil.Domain.Localization;

namespace Tahil.Application.Auth.Queries;

public record LoginQuery(LoginParams LoginModel) : IQuery<Result<LoginResult>>;

public class LoginQueryHandler(IUserRepository userRepository, ITokenService tokenService, LocalizedStrings locale) : IQueryHandler<LoginQuery, Result<LoginResult>>
{
    public async Task<Result<LoginResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(r => r.IsActive && r.Email.Value == request.LoginModel.EmailOrPhone || r.PhoneNumber == request.LoginModel.EmailOrPhone);
        
        if (user is null || !PasswordHasher.Verify(request.LoginModel.Password, user.Password))
            throw new DomainException(locale.InvalidCredentials);

        var authModel = new LoginResult();
        authModel.User = user.Adapt<UserDto>();
        authModel.User.Password = null!;
        authModel.Token = tokenService.GenerateToken(user);
        authModel.RefreshToken = tokenService.GenerateRefreshToken();

        return Result.Success(new LoginResult());
    }
}