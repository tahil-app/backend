namespace Tahil.Application.Services;

public class ConfigService(IConfiguration configuration) : IConfigService
{
    //public int StoreOrderQuota => int.TryParse(GetConfiguration("StoreOrderQuota"), out int result) ? result : 500;
    
    public string EmailSender => GetConfiguration("Email:Sender") ?? "";
    public string AppName => GetConfiguration("AppName") ?? "";
    public string LogoUrl => GetConfiguration("LogoUrl") ?? "";
    public string ResetUrl => GetConfiguration("ResetUrl") ?? "";

    private string? GetConfiguration(string str)
    {
        return Environment.GetEnvironmentVariable(str) ?? configuration[str];
    }
}