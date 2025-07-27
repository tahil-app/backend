using Mapster;
using Tahil.Domain.Dtos;

namespace Tahil.Infrastructure.Repositories;

public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(BEContext context) : base(context.Set<Attachment>())
    {
    }

    public Attachment AddAttachment(AttachmentDto attachmentDto, string userName)
    {
        var attachment = attachmentDto.Adapt<Attachment>();
        attachment.CreatedOn = DateTime.Now;
        attachment.CreatedBy = userName;

        Add(attachment);

        return attachment;
    }

    public async Task<Attachment> RemoveAttachment(int attachmentId) 
    {
        var deletedAttach = await GetAsync(r => r.Id == attachmentId);
        HardDelete(deletedAttach!);

        return deletedAttach;
    }
}