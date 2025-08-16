using Mapster;
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

    public Attachment AddAttachment(AttachmentDto attachmentDto, string userName, Guid tenantId)
    {
        var attachment = attachmentDto.Adapt<Attachment>();
        attachment.CreatedOn = DateTime.Now;
        attachment.CreatedBy = userName;
        attachment.TenantId = tenantId;

        Add(attachment);

        return attachment;
    }

    public async Task<Result<Attachment>> RemoveAttachment(int attachmentId, Guid tenantId) 
    {
        var attachment = await GetAsync(r => r.Id == attachmentId && r.TenantId == tenantId);
        if (attachment is null)
            return Result<Attachment>.Failure(_localizedStrings.NotFoundAttachment);

        HardDelete(attachment!);

        return Result<Attachment>.Success(attachment);
    }

    public async Task<bool> ExistsInTenantAsync(int id, Guid tenantId)
    {
        return await _dbSet.AnyAsync(c => c.Id == id && c.TenantId == tenantId);
    }

}