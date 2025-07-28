namespace Tahil.Domain.Services;

public interface IUploadService
{
    Task<string> UploadAsync(string folderPath, Guid guid, string fileName, Stream stream);
    Task<bool> DeleteAsync(string filePath);
}
