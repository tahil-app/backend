using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class ReportEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var reports = app.MapGroup("/reports");

        reports.MapGet("/download/{type:int}", async (ReportType type, IWebHostEnvironment env, IReportService reportService) =>
        {
            var teacher = new TeacherDto();

            var report = await reportService.GenerateAsync(type, teacher);

            return Results.File(report, "application/pdf", "student-report.pdf");
        });

        reports.MapGet("/view/{type:int}", async (ReportType type, IWebHostEnvironment env, IReportService reportService) =>
        {
            var teacher = new TeacherDto();

            var report = await reportService.GenerateAsync(type, teacher);

            return Results.File(report, "application/pdf");
        });
    }

    private static bool LocateReport(string fileName, IWebHostEnvironment env, out string? fullPath, out string? contentType, out string userType)
    {
        fullPath = null;
        contentType = null;
        userType = "default";

        try
        {
            // Look for the file in the wwwroot directory
            var filePath = Path.Combine(env.WebRootPath, "reports", fileName);
            
            if (File.Exists(filePath))
            {
                fullPath = filePath;
                contentType = GetContentType(fileName);
                return true;
            }

            // If not found in reports folder, try attachments folder
            filePath = Path.Combine(env.WebRootPath, "attachments", fileName);
            if (File.Exists(filePath))
            {
                fullPath = filePath;
                contentType = GetContentType(fileName);
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".xls" => "application/vnd.ms-excel",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".doc" => "application/msword",
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            _ => "application/octet-stream"
        };
    }
}
