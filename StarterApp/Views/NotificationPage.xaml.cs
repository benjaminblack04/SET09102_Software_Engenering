using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class NotificationPage : ContentPage
{
    public NotificationPage(NotificationPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}