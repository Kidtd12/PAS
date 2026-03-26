using Application.Common.Interfaces;
using Hangfire;

namespace PAS.API.BackgroundServices;

public class NotificationCleanupService : BackgroundService
{
    private readonly ILogger<NotificationCleanupService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public NotificationCleanupService(ILogger<NotificationCleanupService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification cleanup service started");

        // Schedule cleanup job to run daily at 2 AM
        RecurringJob.AddOrUpdate("cleanup-old-notifications", () => CleanupOldNotifications(), Cron.Daily(2));

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    public async Task CleanupOldNotifications()
    {
        _logger.LogInformation("Cleaning up old notifications");

        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var cutoffDate = DateTime.UtcNow.AddMonths(-3);
        var oldNotifications = await context.Notifications
            .Where(n => n.SentDate < cutoffDate && n.IsRead)
            .ToListAsync();

        foreach (var notification in oldNotifications)
        {
            notification.SoftDelete();
        }

        await context.SaveChangesAsync();
        _logger.LogInformation("Cleaned up {Count} old notifications", oldNotifications.Count);
    }
}