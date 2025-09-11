using Tahil.API.Authorization;
using Tahil.Application.StudentAttendancs.Commands;
using Tahil.Application.StudentAttendancs.Queries;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class StudentAttendanceEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var attendances = app.MapGroup("/student_attendances");

        #region Get Attendances

        attendances.MapGet("/{sessionId:int}", async (int sessionId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetStudentAttendancesQuery(sessionId));
            return Results.Ok(result);
        }).RequireAccess(EntityType.StudentAttendance, AuthorizationOperation.ViewDetail, "sessionId");

        attendances.MapGet("/monthly/{studentId:int}/{year:int}", async (int studentId, int year, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetStudentMonthlyAttendanceQuery(studentId, year));
            return Results.Ok(result);
        }).RequireAccess(EntityType.StudentAttendance, AuthorizationOperation.ViewAll);

        attendances.MapGet("/daily/{studentId:int}/{year:int}/{month:int}", async (int studentId, int year, int month, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetStudentDailyAttendanceQuery(studentId, year, month));
            return Results.Ok(result);
        }).RequireAccess(EntityType.StudentAttendance, AuthorizationOperation.ViewAll);

        #endregion

        #region Reports

        attendances.MapGet("/monthly-report/{id:int}/{year:int}", async (int id, int year, IReportService reportService) =>
        {
            var report = await reportService.GenerateAsync(ReportType.StudentAttendnceMonthly, new { Id = id, Year = year });

            return Results.File(report, "application/pdf");
        }).RequireAccess(EntityType.Student, AuthorizationOperation.ViewDetail);

        //students.MapGet("/daily-report/{id:int}/{year:int}/{month:int}", async (int id, int year, int month, IReportService reportService) =>
        //{
        //    var report = await reportService.GenerateAsync(ReportType.StudentFeedback, new { Id = id, Year = year, Month = month });

        //    return Results.File(report, "application/pdf");
        //}).RequireAccess(EntityType.Student, AuthorizationOperation.ViewDetail);

        #endregion

        #region Update

        attendances.MapPut("/update/{sessionId:int}", async (int sessionId, List<StudentAttendanceDto> model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateStudentAttendanceCommand(sessionId, model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.StudentAttendance, AuthorizationOperation.Update, "sessionId");

        #endregion
    }
}