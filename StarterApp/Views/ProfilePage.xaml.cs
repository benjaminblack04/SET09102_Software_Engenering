using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfilePageModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}