using Quartz;
using Tahil.API.Authorization;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class BackgroundJobEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var jobs = app.MapGroup("/jobs");

        jobs.MapPost("/trigger-class-session-generation", async ([FromServices] ISchedulerFactory schedulerFactory) =>
        {
            try
            {
                var scheduler = await schedulerFactory.GetScheduler();
                var jobKey = new JobKey("classSessionGenerationJob", "dailyJobs");
                
                // Trigger the job immediately
                await scheduler.TriggerJob(jobKey);
                
                return Results.Ok(new { message = "Class session generation job triggered successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }).RequireAccess(EntityType.BackgroundJob, AuthorizationOperation.Update);
    }
} 