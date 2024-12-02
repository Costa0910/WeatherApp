using MobileApp.Pages;
using MobileApp.Services;

namespace MobileApp;

public partial class App : Application
{
    public App(ApiService service)
    {
        InitializeComponent();
        // Check if token exists, if so, set AppShell as the main page else set LoginPage
        var token = Preferences.Get("Token", string.Empty);
        if (!string.IsNullOrEmpty(token))
        {
            MainPage = new AppShell(service);
        }
        else
        {
            MainPage = new LoginPage(service);
        }

        // MainPage = new AppShell();
    }
}
