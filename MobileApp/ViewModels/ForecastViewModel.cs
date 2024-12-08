using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileApp.Models;
using MobileApp.Services;

namespace MobileApp.ViewModels;

public class ForecastViewModel : BaseViewModel<WeatherForecastModel>
{

    public ForecastViewModel(ApiService service)
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

            var (lat, lon) = await GetUserLocationAsync();
            if (lat == null || lon == null)
            {
                throw new Exception(
                    "Unable to get location. Please enable location services.");
            }

            var url = $"data/2.5/forecast?lat={lat}&lon={lon}&cnt=7";
            var response = await _service.GetWeatherAsync<WeatherForecastModel>(url);

            if (response.HasError || response.Data == null)
            {
                throw new Exception(response.ErrorMessage ??
                                    "Failed to fetch forecast weather data.");
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
