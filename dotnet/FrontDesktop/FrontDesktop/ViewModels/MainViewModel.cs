using CommunityToolkit.Mvvm.ComponentModel;

namespace FrontDesktop.ViewModels;

public partial class MainViewModel(
) : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";

    public async Task ExecuteEventsAsync()
    {
    }

    public async Task ExecuteCrudAsync()
    {
    }

    public async Task ExecuteStorageAsync()
    {
    }
}
