using Microsoft.AspNetCore.StaticFiles;

namespace Tahil.API.Endpoints;

public class AttachmentEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var attachments = app.MapGroup("/attachment");

        attachments.MapGet("/download/{fileName}", async (string fileName, IWebHostEnvironment env, IAttachmentService attachmentService) =>
        {
            var result = LocateAttachment(fileName, env, out var fullPath, out var contentType, out var userType);

            var displayName = await attachmentService.GetAttachmentDisplayNameAsync(userType, fileName);

            var downloadName = $"{displayName}{Path.GetExtension(fileName)}";

            return result switch
            {
                true => Results.File(fullPath!, contentType!, downloadName), // Force download
                false => Results.NotFound($"File '{fileName}' not found.")
            };
        }).RequireAuthorization(Policies.ALL);

        attachments.MapGet("/view/{fileName}", (string fileName, IWebHostEnvironment env) =>
        {
            var result = LocateAttachment(fileName, env, out var fullPath, out var contentType, out var userType);

            return result switch
            {
                true => Results.File(fullPath!, contentType!), // View in browser
                false => Results.NotFound($"File '{fileName}' not found.")
            };
        }).RequireAuthorization(Policies.ALL);
    }

    private static bool LocateAttachment(string fileName, IWebHostEnvironment env, out string? fullPath, out string? contentType, out string? userType)
    {
        fullPath = null;
        contentType = null;
        userType = null;

        var attachmentsRoot = Path.Combine(env.WebRootPath, "attachments");
        var searchDirs = Directory.GetDirectories(attachmentsRoot, "*", SearchOption.AllDirectories);

        foreach (var dir in searchDirs)
        {
            var potentialPath = Path.Combine(dir, fileName);
            if (File.Exists(potentialPath))
            {
                if (dir.Contains("\\Students\\") || dir.Contains("/Students/"))
                    userType = "Students";
                else if (dir.Contains("\\Teachers\\") || dir.Contains("/Teachers/"))
                    userType = "Teachers";

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileName, out contentType))
                    contentType = "application/octet-stream";

                fullPath = potentialPath;
                return true;
            }
        }

        return false;
    }
}
