namespace Tahil.EmailSender.Services;

public interface IEmailSenderService
{
    Task<bool> SendEmailAsync(string to, string subject, string htmlBody, string? plainTextBody = null);
}
