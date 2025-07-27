namespace Tahil.Domain.Services;

public interface IUploadService
{
    Task<string> UploadAsync(string path, Guid guid, string fileName, string extension, Stream stream);
    Task<bool> DeleteAsync(string path);
}
