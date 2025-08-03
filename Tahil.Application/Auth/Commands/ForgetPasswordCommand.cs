using Tahil.Domain.Localization;
using Tahil.EmailSender.Helpers;
using Tahil.EmailSender.Services;

namespace Tahil.Application.Auth.Commands;

public record ForgetPasswordCommand(string Email) : ICommand<Result<bool>>;

public class ForgetPasswordCommandHandler(
    IUserRepository userRepository,
    IEmailSenderService emailSenderService,
    IConfigService configService,
    LocalizedStrings locale) : ICommandHandler<ForgetPasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(r => r.IsActive && r.Email.Value == request.Email, [r => r.Tenant!]);
        if (user is null)
            return Result<bool>.Failure(locale.InvalidCredentials);


        var placeholders = new Dictionary<string, string>
        {
            { "Logo", user.TenantId.HasValue && user.Tenant!.LogoUrl!.Any() ? user.Tenant!.LogoUrl! : configService.LogoUrl },
            { "ResetLink", configService.ResetUrl },
            { "Year", DateTime.UtcNow.Year.ToString() },
            { "Company", user.TenantId.HasValue ? user.Tenant!.Name : configService.AppName}
        };

        var parsedHtml = await EmailTemplateHelper.GetParsedTemplateAsync(Path.Combine(AppContext.BaseDirectory, "Templates"), "ForgetPassword.html", placeholders);

        var result = await emailSenderService.SendEmailAsync(request.Email, "Reset your password", parsedHtml);

        return Result.Success(result);
    }
}
