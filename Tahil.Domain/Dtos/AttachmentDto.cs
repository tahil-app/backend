namespace Tahil.Domain.Dtos;

public class AttachmentDto: BaseDto
{
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    public Guid? FileId { get; set; }
    public string? MimeType { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
}