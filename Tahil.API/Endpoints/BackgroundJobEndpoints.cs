using Quartz;
using Tahil.API.Authorization;
using Tahil.Common.Contracts;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class BackgroundJobEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var jobs = app.MapGroup("/jobs");

        jobs.MapPost("/refresh-sessions", async ([FromServices] ISchedulerFactory schedulerFactory) =>
        {
            try
            {
                var scheduler = await schedulerFactory.GetScheduler();
                var jobKey = new JobKey("classSessionGenerationJob", "dailyJobs");
                
                // Trigger the job immediately
                await scheduler.TriggerJob(jobKey);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }

        }).RequireAccess(EntityType.BackgroundJob, AuthorizationOperation.Update);
    }
} 