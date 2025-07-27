namespace Tahil.Domain.Services;

public interface IAttachmentService
{
    Task<Attachment> AddAsync(string path, string originalFileName, string uploadFileName, Stream stream);
    Task<bool> DeleteAsync(int id, string path);
}