using MobileApp.Pages;
using MobileApp.Services;
using MobileApp.ViewModels;

namespace MobileApp;

public partial class AppShell : Shell
{
    private readonly ApiService _service;
    private readonly CurrentWeatherViewModel _viewModel;

    public AppShell(ApiService service, CurrentWeatherViewModel viewModel)
    {
        _service = service;
        _viewModel = viewModel;

        InitializeComponent();
        ConfigureShell();
    }

    private void ConfigureShell()
    {
        // Add each page as FlyoutItem
        Items.Add(CreateFlyoutItem("Current Weather", "current_weather_icon.svg", new CurrentWeatherPage(_viewModel)));
        Items.Add(CreateFlyoutItem("Forecast", "forecast_icon.svg", new ForecastPage()));
        Items.Add(CreateFlyoutItem("Search", "search_icon.svg", new SearchPage()));
        Items.Add(CreateFlyoutItem("Favorites", "favorites_icon.svg", new FavoritesPage()));
        Items.Add(CreateFlyoutItem("About", "about_icon.svg", new AboutPage()));

        // Add Logout MenuItem
        var logoutMenuItem = new MenuItem
        {
            Text = "Logout",
            IconImageSource = "logout_icon.svg",
            Command = new Command(async () =>
            {
                var confirm = await Application.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to logout?", "OK", "Cancel");
                if (!confirm) return;
                Preferences.Clear();
                Application.Current.MainPage = new LoginPage(_service, _viewModel);
            })
        };

        Items.Add(logoutMenuItem);
    }

    private FlyoutItem CreateFlyoutItem(string title, string icon, Page page)
    {
        return new FlyoutItem
        {
            Title = title,
            Icon = icon,
            Items =
            {
                new ShellContent
                {
                    ContentTemplate = new DataTemplate(() => page),
                    Title = title // For better navigation labels
                }
            }
        };
    }
}
