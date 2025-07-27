using Tahil.EmailSender.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Tahil.EmailSender.Services;

public class EmailSenderService(IOptions<EmailSettings> emailOptions) : IEmailSenderService
{
    private readonly EmailSettings emailSettings = emailOptions.Value;
    
    public async Task<bool> SendEmailAsync(string to, string subject, string htmlBody, string? plainTextBody = null)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(emailSettings.From));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = plainTextBody
            };

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailSettings.Username, emailSettings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            return true; // success
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email send failed: {ex.Message}");
            return false; // failure
        }
    }

}