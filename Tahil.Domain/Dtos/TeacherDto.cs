namespace Tahil.Domain.Dtos;

public class TeacherDto : UserDto
{
    public string Experience { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;

    public List<AttachmentDto> Attachments { get; set; } = new();
}