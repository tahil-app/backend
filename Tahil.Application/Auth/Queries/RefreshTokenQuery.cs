using Tahil.Application.Auth.Models;

namespace Tahil.Application.Auth.Queries;

public record RefreshTokenQuery(string Token) : IQuery<LoginResult>;

public class RefreshTokenHandler(ITeacherRepository userRepository, ITokenService tokenService) : IQueryHandler<RefreshTokenQuery, LoginResult>
{
    public async Task<LoginResult> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.Token);
        var email = principal.Claims.FirstOrDefault(r => r.Type.Contains("emailaddress"));
        if (email is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        //var user = await userRepository.GetAsync(r => r.Email.Value == email.Value);
        //if (user is null)
        //    throw new UnauthorizedAccessException("Invalid refresh token.");

        //user.Password = null!;

        return new LoginResult
        {
            //Token = tokenService.GenerateToken(user),
            //RefreshToken = tokenService.GenerateRefreshToken(),
            //User = user.Adapt<UserDto>()
        };
    }
}
