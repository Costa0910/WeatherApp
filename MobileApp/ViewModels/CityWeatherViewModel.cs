using MobileApp.Models;
using MobileApp.Services;

namespace MobileApp.ViewModels;

public class CityWeatherViewModel : BaseViewModel<WeatherModel>
{
    public double?
        Latitude
    {
        get;
        set;
    } // Data to be given by CityWeatherPage behind the code

    public double? Longitude { get; set; }

    public CityWeatherViewModel(ApiService service)
    {
        _service = service;
        RefreshWeatherCommand
            = new Command(async () => await FetchWeatherAsync(true));
    }

    protected override async Task FetchWeatherAsync(bool isManualRefresh)
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            // If not a manual refresh, use cached data
            if (!isManualRefresh && _modelData != null)
            {
                return;
            }

            if (Longitude == null || Latitude == null)
            {
                throw new Exception(
                    "Unable to get location. Please enable location services.");
            }

            var url = $"data/2.5/weather?lat={Latitude}&lon={Longitude}";
            var response = await _service.GetWeatherAsync<WeatherModel>(url);

            if (response.HasError || response.Data == null)
            {
                throw new Exception(response.ErrorMessage ??
                                    "Failed to fetch weather data.");
            }

            // Update data and timestamp
            ModelData = response.Data;
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
}
