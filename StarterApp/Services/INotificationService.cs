using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface INotificationService
{
    Task<NotificationResult> SendNotificationAsync(string name, string message);
}