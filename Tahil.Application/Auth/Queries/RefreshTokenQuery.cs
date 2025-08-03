using Tahil.Application.Auth.Models;
using Tahil.Domain.Localization;

namespace Tahil.Application.Auth.Queries;

public record RefreshTokenQuery(string Token) : IQuery<LoginResult>;

public class RefreshTokenHandler(IUserRepository userRepository, ITokenService tokenService, LocalizedStrings locale) : IQueryHandler<RefreshTokenQuery, LoginResult>
{
    public async Task<LoginResult> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.Token);
        var email = principal.Claims.FirstOrDefault(r => r.Type.Contains("emailaddress"));
        if (email is null)
            throw new UnauthorizedAccessException(locale.InvalidRefreshToken);

        var user = await userRepository.GetAsync(r => r.Email.Value == email.Value);
        if (user is null)
                throw new UnauthorizedAccessException(locale.InvalidRefreshToken);

        user.Password = null!;

        return new LoginResult
        {
            Token = tokenService.GenerateToken(user),
            RefreshToken = tokenService.GenerateRefreshToken(),
            User = user.Adapt<UserDto>()
        };
    }
}
