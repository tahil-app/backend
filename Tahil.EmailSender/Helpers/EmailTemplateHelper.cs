namespace Tahil.EmailSender.Helpers;

public static class EmailTemplateHelper
{
    public static async Task<string> GetParsedTemplateAsync(string folderPath, string fileName, Dictionary<string, string> placeholders)
    {
        var fullPath = Path.Combine(folderPath, fileName);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Template file '{fileName}' not found at '{folderPath}'.");

        var templateContent = await File.ReadAllTextAsync(fullPath);

        foreach (var placeholder in placeholders)
        {
            templateContent = templateContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
        }

        return templateContent;
    }
}
