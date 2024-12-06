using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MobileApp.Models;
using MobileApp.Services;

namespace MobileApp.ViewModels;

public class CurrentWeatherViewModel : INotifyPropertyChanged
{
    private readonly ApiService _service;
    private bool _isLoading;
    private string _errorMessage;
    private WeatherModel _currentWeather;
    private DateTime _lastFetchTime;

    public event PropertyChangedEventHandler? PropertyChanged;
    public DateTime CurrentDateTime => DateTime.Now;

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
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

    public WeatherModel CurrentWeather
    {
        get => _currentWeather;
        set
        {
            _currentWeather = value;
            OnPropertyChanged();
        }
    }

    public string LastUpdatedText
    {
        get => _lastFetchTime == DateTime.MinValue
            ? "Never updated"
            : $"Last updated: {_lastFetchTime:G}";
    }

    public ICommand RefreshWeatherCommand { get; }

    public CurrentWeatherViewModel(ApiService service)
    {
        _service = service;
        RefreshWeatherCommand
            = new Command(async () => await FetchWeatherAsync(true));
    }

    public async Task InitializeAsync()
    {
        // Fetch data only if the cache is older than 10 minutes
        if (DateTime.Now - _lastFetchTime > TimeSpan.FromMinutes(10))
        {
            await FetchWeatherAsync(false);
        }
    }

    private async Task FetchWeatherAsync(bool isManualRefresh)
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            // If not a manual refresh, use cached data
            if (!isManualRefresh && _currentWeather != null)
            {
                return;
            }

            var (lat, lon) = await GetUserLocationAsync();
            if (lat == null || lon == null)
            {
                throw new Exception(
                    "Unable to get location. Please enable location services.");
            }

            var url = $"data/2.5/weather?lat={lat}&lon={lon}";
            var response = await _service.GetWeatherAsync(url);

            if (response.HasError || response.Data == null)
            {
                throw new Exception(response.ErrorMessage ??
                                    "Failed to fetch weather data.");
            }

            // Update data and timestamp
            CurrentWeather = response.Data;
            _lastFetchTime = DateTime.Now;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(LastUpdatedText));
        }
    }

    private async Task<(double? lat, double? lon)> GetUserLocationAsync()
    {
        try
        {
            var status
                = await Permissions
                    .RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                throw new Exception("Location permission denied.");
            }

            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location != null)
            {
                return (location.Latitude, location.Longitude);
            }

            throw new Exception("Failed to get the current location.");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void OnPropertyChanged(
        [CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }
}
