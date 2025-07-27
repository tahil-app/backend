using Microsoft.AspNetCore.Http;

namespace Tahil.Application.Teachers.Models;

public record TeacherAttachmentModel
{
    public int TeacherId { get; set; }
    public string Title { get; set; } = default!;
    public IFormFile File { get; set; } = default!;
}