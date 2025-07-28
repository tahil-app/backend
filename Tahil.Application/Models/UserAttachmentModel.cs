using Microsoft.AspNetCore.Http;

namespace Tahil.Application.Models;

public record UserAttachmentModel
{
    public int UserId { get; set; }
    public string DisplayName { get; set; } = default!;
    public IFormFile File { get; set; } = default!;
}

