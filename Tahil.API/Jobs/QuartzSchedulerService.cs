using Quartz;

namespace Tahil.API.Jobs;

public class QuartzSchedulerService : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<QuartzSchedulerService> _logger;
    private IScheduler? _scheduler;

    public QuartzSchedulerService(
        ISchedulerFactory schedulerFactory,
        ILogger<QuartzSchedulerService> logger)
    {
        _schedulerFactory = schedulerFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            
            // Create the job
            var job = JobBuilder.Create<ClassSessionGenerationService>()
                .WithIdentity("classSessionGenerationJob", "dailyJobs")
                .Build();

            // Create the trigger - runs every day at 1:00 AM
            var trigger = TriggerBuilder.Create()
                .WithIdentity("classSessionGenerationTrigger", "dailyJobs")
                .WithCronSchedule("0 0 1 * * ?") // Cron expression: every day at 1:00 AM
                .Build();

            // Schedule the job
            await _scheduler.ScheduleJob(job, trigger, cancellationToken);
            
            // Start the scheduler
            await _scheduler.Start(cancellationToken);
            
            _logger.LogInformation("Quartz scheduler started successfully. Class session generation job scheduled for daily execution at 1:00 AM.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting Quartz scheduler");
            throw;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_scheduler != null)
        {
            await _scheduler.Shutdown(cancellationToken);
            _logger.LogInformation("Quartz scheduler stopped.");
        }
    }
} 