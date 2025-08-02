using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using System.Windows.Input;

namespace StarterApp.ViewModels;

/// @brief View model for the About page that displays application information
/// @details Provides basic application information including name, version, and links to more information
public partial class ProfilePageModel : BaseViewModel
{
    /// @brief Gets the application version from AppInfo
    /// @return The application version string
    public string Version => AppInfo.VersionString;

    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private string lastName = string.Empty;
    
    private readonly IAuthenticationService _authService;

    /// @brief Initializes a new instance of the AboutViewModel class
    /// @details Sets up the ShowMoreInfoCommand with async relay command
    public ProfilePageModel()
    {
        Title = "Profile";
        ClearError();
    }

    public ProfilePageModel(IAuthenticationService authService)
    {
        _authService = authService;

        Title = "Profile";
        ClearError();

        firstName = authService.CurrentUser.FirstName;
        lastName = authService.CurrentUser.LastName;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
            return;

        if (!ValidateForm())
            return;

        try
        {
            IsBusy = true;
            ClearError();

            var result_fn = await _authService.SetFirstnameAsync(firstName);
            var result_ln = await _authService.SetLastnameAsync(lastName);

            if (result_fn.IsSuccess || result_ln.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Profile changed succesfully!.", "OK");
            }
            else
            {
                SetError("Couldn't save profile!");
            }
        }
        catch (Exception ex)
        {
            SetError($"Save failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            SetError("First name is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(LastName))
        {
            SetError("Last name is required");
            return false;
        }

        return true;
    }
}