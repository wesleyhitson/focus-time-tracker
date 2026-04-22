namespace FocusTimeTracker.Services;

public interface INotificationService
{
    Task ShowNotificationAsync(string title, string message);
    Task<bool> RequestPermissionsAsync();
}
