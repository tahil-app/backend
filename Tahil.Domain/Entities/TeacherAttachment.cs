namespace Tahil.Domain.Entities;

public class TeacherAttachment: Base
{
    public int TeacherId { get; set; }
    public int AttachmentId { get; set; }
    public string DisplayName { get; set; } = default!;

    public Teacher Teacher { get; set; } = default!;
    public Attachment Attachment { get; set; } = default!;
}