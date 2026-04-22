using System.Threading.Tasks;

namespace FocusTimeTracker.Services;

public class NotificationService : INotificationService
{
    public Task<bool> RequestPermissionsAsync()
    {
        return Task.FromResult(true);
    }

    public Task ShowNotificationAsync(string title, string message)
    {
        System.Console.WriteLine($"Notification: {title} - {message}");
        return Task.CompletedTask;
    }
}
