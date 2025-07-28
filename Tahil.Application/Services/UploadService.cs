namespace Tahil.Application.Services;

public class UploadService: IUploadService
{
    public async Task<string> UploadAsync(string folderPath, Guid guid, string fileName, Stream stream)
    {
        Directory.CreateDirectory(folderPath); // Ensure directory exists

        string guidFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-{guid}{Path.GetExtension(fileName)}";

        string directoryPath = Path.GetDirectoryName(folderPath)!;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = Path.Combine(folderPath, guidFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
        }

        return guidFileName;
    }

    public async Task<bool> DeleteAsync(string filePath)
    {
        if (!File.Exists(filePath))
            return true;

        try
        {
            await Task.Run(() => File.Delete(filePath));
            return true;
        }
        catch
        {
            return false;
        }
    }

}
