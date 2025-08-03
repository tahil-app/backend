namespace Tahil.Domain.Repositories;

public interface IAttachmentRepository: IRepository<Attachment>
{
    Attachment AddAttachment(AttachmentDto attachmentDto, string userName, Guid tenantId);
    Task<Result<Attachment>> RemoveAttachment(int attachmentId);
}