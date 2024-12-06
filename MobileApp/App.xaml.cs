using MobileApp.Pages;
using MobileApp.Services;
using MobileApp.ViewModels;

namespace MobileApp;

public partial class App : Application
{
    public App(ApiService service, CurrentWeatherViewModel viewModel)
    {
        InitializeComponent();
        // Check if token exists, if so, set AppShell as the main page else set LoginPage
        var token = Preferences.Get("Token", string.Empty);
        if (!string.IsNullOrEmpty(token))
        {
            MainPage = new AppShell(service, viewModel);
        }
        else
        {
            MainPage = new LoginPage(service, viewModel);
        }

        // MainPage = new AppShell();
    }
}
