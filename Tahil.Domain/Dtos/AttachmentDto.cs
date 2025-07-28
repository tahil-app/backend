namespace Tahil.Domain.Dtos;

public class AttachmentDto: BaseDto
{
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    public string? DisplayName { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
}