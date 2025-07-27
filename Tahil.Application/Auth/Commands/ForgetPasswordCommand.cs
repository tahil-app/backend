using Tahil.EmailSender.Helpers;
using Tahil.EmailSender.Services;

namespace Tahil.Application.Auth.Commands;

public record ForgetPasswordCommand(string Email) : ICommand<Result<bool>>;

public class ForgetPasswordCommandHandler(IEmailSenderService emailSenderService, IConfigService configService) : ICommandHandler<ForgetPasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        var placeholders = new Dictionary<string, string>
        {
            { "Logo", configService.LogoUrl },
            { "ResetLink", configService.ResetUrl },
            { "Year", DateTime.UtcNow.Year.ToString() },
            { "Company", "Heikal"}
        };

        var parsedHtml = await EmailTemplateHelper.GetParsedTemplateAsync(Path.Combine(AppContext.BaseDirectory, "Templates"), "ForgetPassword.html", placeholders);

        var result = await emailSenderService.SendEmailAsync(request.Email, "Reset your password", parsedHtml);

        return Result<bool>.Success(result);
    }
}
