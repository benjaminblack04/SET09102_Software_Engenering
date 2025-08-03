using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StarterApp.ViewModels;

/// @brief View model for the About page that displays application information
/// @details Provides basic application information including name, version, and links to more information
public partial class ProfilePageModel : BaseViewModel
{
    /// @brief Gets the application version from AppInfo
    /// @return The application version string
    public string Version => AppInfo.VersionString;

    private bool _isRefreshing = false;
    private bool _isLoading = false;

    public bool IsAdmin => _authService.HasRole(RoleConstants.Admin);

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            _isRefreshing = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> RoleFilterOptions { get; }

    public ObservableCollection<EventItem> Events
    {
        get => _events;
        set
        {
            _events = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<EventItem> _events = new();

    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private string lastName = string.Empty;

    private readonly IAuthenticationService _authService;

    private readonly AppDbContext _context;

    public ProfilePageModel()
    {
        Title = "Profile";
        ClearError();
    }

    public ProfilePageModel(AppDbContext context, IAuthenticationService authService)
    {
        _context = context;
        _authService = authService;

        Title = "Profile";
        ClearError();

        firstName = authService.CurrentUser.FirstName;
        lastName = authService.CurrentUser.LastName;

        RefreshCommand = new Command(async () => await RefreshEventsAsync());
        LoadEventsCommand = new Command(async () => await LoadEventsAsync());

        RoleFilterOptions = new ObservableCollection<string> { "All" };
        foreach (var role in RoleConstants.AllRoles)
        {
            RoleFilterOptions.Add(role);
        }

        // Load events when view model is created
        _ = Task.Run(LoadEventsAsync);
    }

    public ICommand RefreshCommand { get; }
    public ICommand LoadEventsCommand { get; }

    private async Task LoadEventsAsync()
    {
        IsLoading = true;
        try
        {
            var events = await _context.Events
                .Where(e => e.Happening >= DateTime.Now)
                .OrderBy(e => e.Happening)
                .ToListAsync();

            _events.Clear();
            foreach (var ev in events)
            {
                var speaker = await _context.Users.FindAsync(ev.SpeakerId);
                // Ok so I messed up the naming of the event and event_attendees tables so they're in different cases for some reason
                var attendeeCount = await _context.EventAttendees.CountAsync(ea => ea.Event_Id == ev.Id);
                if (speaker == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Speaker with ID {ev.SpeakerId} not found for event {ev.Name}");
                    continue;
                }
                // Make sure the current user is at the event, or the user is an admin 'cause we want admins to see all events
                var isAttending = await _context.EventAttendees.AnyAsync(ea => ea.Event_Id == ev.Id &&
                                                                         ea.Attendee_Id == _authService.CurrentUser.Id);
                if (!IsAdmin && !isAttending)
                    continue;
                var eventItem = new EventItem
                {
                    SpeakerName = $"Speaker: {speaker.FullName}",
                    AttendeeCount = attendeeCount,
                    EventName = ev.Name,
                    EventDateTime = ev.Happening.ToString("f"),
                    EventType = ev.Type.ToString()
                };
                // Is the current user the speaker of the event
                if (ev.SpeakerId == _authService.CurrentUser.Id)
                {
                    eventItem.SpeakerName = "Speaker: You!";
                    eventItem.IsSpeaker = "You are the speaker of this event!";
                }
                _events.Add(eventItem);
            }
        }
        catch (Exception ex)
        {
            SetError($"Sorry, we're having some issues loading events. Please try again later.");
            System.Diagnostics.Debug.WriteLine($"Error loading events: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task RefreshEventsAsync()
    {
        IsRefreshing = true;
        await LoadEventsAsync();
        IsRefreshing = false;
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class EventItem
{
    public string SpeakerName { get; set; } = string.Empty;
    public int AttendeeCount { get; set; } = 0;
    public string EventName { get; set; } = string.Empty;
    public string EventDateTime { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string IsSpeaker { get; set; } = string.Empty;
}