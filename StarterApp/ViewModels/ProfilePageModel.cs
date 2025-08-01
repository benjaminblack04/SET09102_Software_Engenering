using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace StarterApp.ViewModels;

/// @brief View model for the About page that displays application information
/// @details Provides basic application information including name, version, and links to more information
public class ProfilePageModel
{
    /// @brief Gets the application title from AppInfo
    /// @return The application name as a string
    public string Title => AppInfo.Name;
    
    /// @brief Gets the application version from AppInfo
    /// @return The application version string
    public string Version => AppInfo.VersionString;
    
    /// @brief Gets the URL for more information about the application
    /// @return URL string pointing to MAUI documentation
    public string MoreInfoUrl => "https://aka.ms/maui";
    
    /// @brief Gets a descriptive message about the application technology stack
    /// @return Description of the app's technology stack
    public string Message => "This app is written in XAML and C# with .NET MAUI.";

    /// @brief Initializes a new instance of the AboutViewModel class
    /// @details Sets up the ShowMoreInfoCommand with async relay command
    public ProfilePageModel()
    {
        
    }
}