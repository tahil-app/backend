namespace Tahil.Domain.Entities;

public class StudentAttachment: Base
{
    public int StudentId { get; set; }
    public int AttachmentId { get; set; }
    public string DisplayName { get; set; } = default!;

    public Student Student { get; set; } = default!;
    public Attachment Attachment { get; set; } = default!;
}