using Tahil.Domain.Enums;
using Tahil.Domain.Localization;

namespace Tahil.API.Endpoints;

public class ReportEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var reports = app.MapGroup("/reports");

        reports.MapGet("/teacher-schedules/{teacherId:int}", async (int teacherId, IReportService reportService, LocalizedStrings localized) =>
        {
            var report = await reportService.GenerateAsync(ReportType.TeacherSchedule, teacherId);

            return Results.File(report, "application/pdf");
        });
    }

}
