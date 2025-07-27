namespace Tahil.Domain.Entities;

public class Attachment: Base
{
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    public Guid? FileId { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
}