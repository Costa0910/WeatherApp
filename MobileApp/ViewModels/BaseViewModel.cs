using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MobileApp.Services;

namespace MobileApp.ViewModels;

public abstract class BaseViewModel<T> : INotifyPropertyChanged
{
    private bool _isLoading;
    private string _errorMessage;
    protected T _modelData;
    protected DateTime _lastFetchTime;
    protected ApiService _service;


    public event PropertyChangedEventHandler? PropertyChanged;
    public DateTime CurrentDateTime => DateTime.Now;
    public ICommand RefreshWeatherCommand { get; set; }

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

    public T ModelData
    {
        get => _modelData;
        set
        {
            _modelData = value;
            OnPropertyChanged();
        }
    }

    public string LastUpdatedText
    {
        get => _lastFetchTime == DateTime.MinValue
            ? "Never updated"
            : $"Last updated: {_lastFetchTime:G}";
    }

    /// <summary>
    ///   Initializes the view model.
    ///  Page classes should call this method in their OnAppearing method.
    /// </summary>
    public async Task InitializeAsync()
    {
        // Fetch data only if the cache is older than 10 minutes
        if (DateTime.Now - _lastFetchTime > TimeSpan.FromMinutes(10))
        {
            await FetchWeatherAsync(false);
        }
    }

    protected abstract Task FetchWeatherAsync(bool isManualRefresh);

    protected async Task<(double? lat, double? lon)> GetUserLocationAsync()
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

    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }
}
