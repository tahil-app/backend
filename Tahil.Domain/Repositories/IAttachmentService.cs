namespace Tahil.Domain.Repositories;

public interface IAttachmentRepository: IRepository<Attachment>
{
    Attachment AddAttachment(AttachmentDto attachmentDto, string userName);
    Task<Attachment> RemoveAttachment(int attachmentId);
}