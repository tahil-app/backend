namespace Tahil.Infrastructure.Repositories;

public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(BEContext context) : base(context.Set<Attachment>())
    {
    }
}