namespace Tahil.Application.Services;

public class UploadService: IUploadService
{
    public async Task<string> UploadAsync(string path, Guid guid, string fileName, string extension, Stream stream)
    {
        Directory.CreateDirectory(path); // Ensure directory exists

        string fullPath = Path.Combine(path, $"{fileName}-{guid}{extension}");

        string directoryPath = Path.GetDirectoryName(fullPath)!;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
        }

        return fileName;
    }

    public async Task<bool> DeleteAsync(string path)
    {
        if (!File.Exists(path))
            return true;

        try
        {
            await Task.Run(() => File.Delete(path));
            return true;
        }
        catch
        {
            return false;
        }
    }

}
