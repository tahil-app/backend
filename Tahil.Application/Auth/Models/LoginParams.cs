namespace Tahil.Application.Auth.Models;

public record LoginParams
{
    public string EmailOrPhone { get; set; } = default!;
    public string Password { get; set; } = default!;
}