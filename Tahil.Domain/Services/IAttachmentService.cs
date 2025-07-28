namespace Tahil.Domain.Services;

public interface IAttachmentService
{
    Task<string> GetAttachmentDisplayNameAsync(string attachmentUserType, string attachmentName);
}