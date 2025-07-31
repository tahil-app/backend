using Mapster;
using Tahil.Common.Exceptions;
using Tahil.Domain.Dtos;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
{
    private readonly LocalizedStrings _localizedStrings;
    public AttachmentRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Attachment>())
    {
        _localizedStrings = localizedStrings;
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
        var attachment = await GetAsync(r => r.Id == attachmentId);
        if (attachment is null)
            throw new NotFoundException(_localizedStrings.NotFoundAttachment);

        HardDelete(attachment!);

        return attachment;
    }
}