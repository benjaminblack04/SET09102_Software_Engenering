using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StarterApp.ViewModels;

/// @brief View model for the About page that displays application information
/// @details Provides basic application information including name, version, and links to more information
public partial class NotificationPageViewModel : BaseViewModel
{
    /// @brief Gets the application version from AppInfo
    /// @return The application version string
    public string Version => AppInfo.VersionString;

    [ObservableProperty]
    private string notificationName = string.Empty;

    [ObservableProperty]
    private string notificationMessage = string.Empty;

    private readonly IAuthenticationService _authService;
    private INotificationService _notifService;

    private readonly AppDbContext _context;

    public NotificationPageViewModel()
    {
        Title = "Notifications";
        ClearError();
    }

    public NotificationPageViewModel(AppDbContext context, IAuthenticationService authService, INotificationService notifService)
    {
        _context = context;
        _authService = authService;
        _notifService = notifService;

        Title = "Notifications";
        ClearError();

        notificationName = "";
        notificationMessage = "";
    }

    [RelayCommand]
    private async Task SendAsync()
    {
        if (IsBusy)
            return;

        if (!ValidateForm())
            return;

        try
        {
            IsBusy = true;
            ClearError();

            var result = await _notifService.SendNotificationAsync(notificationName, notificationMessage);

            if (!result.IsSuccess)
                throw new Exception(result.Message);
            await Application.Current.MainPage.DisplayAlert("Success", "Notification sent successfully! (check the console output)", "OK");
        }
        catch (Exception ex)
        {
            SetError($"Notification failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(NotificationName))
        {
            SetError("Notification name is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(NotificationMessage))
        {
            SetError("Message is required");
            return false;
        }

        return true;
    }
}