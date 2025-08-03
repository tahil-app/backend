namespace Tahil.Domain.Entities;

public class Attachment: Base
{
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}