using MobileApp.Services;
using MobileApp.ViewModels;

namespace MobileApp.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly CurrentWeatherViewModel _viewModel;
    private readonly ForecastViewModel _forecastViewModel;
    private readonly FavoriteService _favoriteService;

    public LoginPage(ApiService apiService, CurrentWeatherViewModel
            viewModel, ForecastViewModel forecastViewModel, FavoriteService favoriteService)
    {
        InitializeComponent();
        _apiService = apiService;
        _viewModel = viewModel;
        _forecastViewModel = forecastViewModel;
        _favoriteService = favoriteService;
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        var emailEntry = this.FindByName<Entry>("EmailEntry");
        var passwordEntry = this.FindByName<Entry>("PasswordEntry");
        var loadingOverlay = this.FindByName<Grid>("LoadingOverlay");

        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        if (IsValidEmail(email) && IsValidPassword(password))
        {
            // Show loading overlay
            loadingOverlay.IsVisible = true;

            // Call the API to authenticate
            var response = await _apiService.Login(email, password);

            // Hide loading overlay
            loadingOverlay.IsVisible = false;

            if (response.Data)
            {
                // Set AppShell as the main page after successful login
                if (Application.Current is App app)
                {
                    app.MainPage = new AppShell(_apiService, _viewModel,
                        _forecastViewModel, _favoriteService);
                }
                else
                {
                    // Show error message
                    await DisplayAlert("Error", "App is not initialized", "OK");
                }
            }
            else
            {
                // Show error message
                await DisplayAlert("Error", response.ErrorMessage, "OK");
            }
        }
        else
        {
            // Show error message
            await DisplayAlert("Error", "Invalid email or password", "OK");
        }
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidPassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Length > 6;
    }
}
