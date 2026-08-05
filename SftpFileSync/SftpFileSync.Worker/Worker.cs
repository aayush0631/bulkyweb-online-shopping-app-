using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SftpFileSync.Worker.Models;
using SftpFileSync.Worker.Services;

namespace SftpFileSync.Worker;

public class Worker : BackgroundService
{
    // Logger used to record the service lifecycle and synchronization events.
    private readonly ILogger<Worker> _logger;

    // Service responsible for performing the actual SFTP synchronization.
    private readonly ISftpSyncService _syncService;

    // Parsed cron expression used to calculate the next scheduled execution time.
    private readonly CronExpression _cron;

    public Worker(
        ILogger<Worker> logger,
        ISftpSyncService syncService,
        IOptions<ScheduleSettings> schedule)
    {
        _logger = logger;
        _syncService = syncService;

        // Read the cron expression from configuration and parse it once.
        // Parsing only once improves performance since the schedule rarely changes.
        _cron = CronExpression.Parse(schedule.Value.Cron);
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker Started");

        // Keep the worker running until Windows requests the service to stop.
        while (!stoppingToken.IsCancellationRequested)
        {
            // Calculate the next execution time based on the cron schedule.
            var next = _cron.GetNextOccurrence(
                DateTimeOffset.Now,
                TimeZoneInfo.Local);

            if (next == null)
                continue;

            // Calculate how long to wait before the next scheduled execution.
            var delay = next.Value - DateTimeOffset.Now;

            if (delay > TimeSpan.Zero)
            {
                await Task.Delay(delay, stoppingToken);
            }

            _logger.LogInformation(
                "Next execution at {time}",
                next.Value);

            // Suspend execution without blocking the thread.
            await Task.Delay(delay, stoppingToken);

            try
            {
                // Execute the file synchronization task.
                await _syncService.ExecuteAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                // Log the failure and continue waiting for the next schedule.
                _logger.LogError(ex,
                    "Synchronization Failed");
            }
        }
    }
}