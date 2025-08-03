namespace Tahil.Domain.Services;

public interface IConfigService
{
    string EmailSender { get; }
    string AppName { get; }
    string LogoUrl { get; }
    string ResetUrl { get; }
}