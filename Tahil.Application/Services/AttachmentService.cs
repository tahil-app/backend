using Tahil.Domain.Entities;

namespace Tahil.Application.Services;

public class AttachmentService(IAttachmentRepository attachmentRepository, IApplicationContext applicationContext) : IAttachmentService
{
    public async Task<Attachment> AddAsync(string path, string originalFileName, string uploadFileName, Stream stream)
    {
        Directory.CreateDirectory(path); // Ensure directory exists

        var guid = Guid.NewGuid();

        string extension = Path.GetExtension(originalFileName);
        string fileName = $"{uploadFileName}-{guid}{extension}";
        string fullPath = Path.Combine(path, fileName);

        string directoryPath = Path.GetDirectoryName(fullPath)!;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
        }

        var attachment = new Attachment 
        {
            FileId = guid,
            FileName = fileName,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = applicationContext.UserName,
            FileSize = stream.Length
        };

        attachmentRepository.Add(attachment);

        return attachment;
    }

    public async Task<bool> DeleteAsync(int id, string path)
    {
        var attachment = await attachmentRepository.GetAsync(r => r.Id == id);
        if (attachment == null) 
            throw new NotFoundException("Attachment");

        attachmentRepository.HardDelete(attachment);

        string fullPath = Path.Combine(path, attachment.FileName!);

        if (!File.Exists(fullPath))
            return true;

        try
        {
            await Task.Run(() => File.Delete(fullPath));
            return true;
        }
        catch
        {
            // Optionally log the error
            return false;
        }
    }

}
