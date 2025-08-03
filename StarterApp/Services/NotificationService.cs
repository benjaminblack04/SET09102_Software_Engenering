using StarterApp.Database.Data;

namespace StarterApp.Services;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;

    public NotificationService(AppDbContext context)
    {
        _context = context;

        // Change notification based on device
        // This should be a switch case
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            NotifSend = SendWindowsNotification;
        }
        else if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            NotifSend = SendAndroidNotification;
        }
        else if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            NotifSend = SendIOSNotification;
        }
    }

    private event Func<string, string, Task<NotificationResult>>? NotifSend;

    private async Task<NotificationResult> SendWindowsNotification(string name, string message) {
        Console.WriteLine($"Sending Windows notification: {name} - {message}");
        return new NotificationResult(true, "Windows notification sent successfully");
    }

    private async Task<NotificationResult> SendAndroidNotification(string name, string message) {
        Console.WriteLine($"Sending Android notification: {name} - {message}");
        return new NotificationResult(true, "Android notification sent successfully");
    }

    private async Task<NotificationResult> SendIOSNotification(string name, string message) {
        Console.WriteLine($"Sending iOS notification: {name} - {message}");
        return new NotificationResult(true, "iOS notification sent successfully");
    }

    public async Task<NotificationResult> SendNotificationAsync(string name, string message)
    {
        try
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(message))
            {
                return new NotificationResult(false, "Name and message cannot be empty");
            }

            NotifSend?.Invoke(name, message);

            return new NotificationResult(true, "Notification sent");
        }
        catch (Exception ex)
        {
            return new NotificationResult(false, $"Notification failed to send: {ex.Message}");
        }
    }
}

public class NotificationResult
{
    public bool IsSuccess { get; }
    public string Message { get; }

    public NotificationResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }
}