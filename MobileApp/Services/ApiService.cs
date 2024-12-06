using System.Net;
using System.Text;
using System.Text.Json;
using MobileApp.Models;
using Microsoft.Extensions.Logging;

namespace MobileApp.Services;

public class ApiService(
    HttpClient httpClient,
    ILogger<ApiService> logger)
{
    private static string BaseUrl = AppConfig.BaseUrl;

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<ApiResponse<WeatherModel>> GetWeatherAsync(string url)
    {
        try
        {
            var (data, errorMessage) = await GetAsync<WeatherModel>(url);

            if (errorMessage != null)
            {
                return new ApiResponse<WeatherModel>
                {
                    ErrorMessage = errorMessage
                };
            }

            return new ApiResponse<WeatherModel> { Data = data };
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                $"Error sending HTTP request: {ex.Message}");
            return new ApiResponse<WeatherModel> { ErrorMessage = ex.Message };
        }
    }


    public async Task<ApiResponse<bool>> Login(string email,
        string password)
    {
        BaseUrl = AppConfig.LoginUrl;
        try
        {
            var login = new LoginModel()
            {
                Email = email,
                Password = password
            };

            var json = JsonSerializer.Serialize(login, _serializerOptions);
            var content
                = new StringContent(json, Encoding.UTF8,
                    "application/json");

            var response = await PostRequest("api/Users/Login", content);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError(
                    $"Error sending HTTP request: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage
                        = $"Error sending HTTP request: {response.StatusCode}"
                };
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result
                = JsonSerializer.Deserialize<TokenModel>(
                    jsonResult,
                    _serializerOptions);

            Preferences.Set("Token", result!.Token);
            Preferences.Set("UserId", result.UserId!);
            Preferences.Set("FullName", result.FullName);

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                $"Error sending HTTP request: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
        finally
        {
            // Reset the base URL, login and weather APIs have different URLs
            BaseUrl = AppConfig.BaseUrl;
        }
    }

    private async Task<(T? Data, string? ErrorMessage)> GetAsync<T>(
        string endpoint)
    {
        try
        {
            var url = BaseUrl + endpoint;
            url = AddAuthorizationHeader(url);

            var response
                = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseString
                    = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(responseString,
                    _serializerOptions);
                return (data ?? Activator.CreateInstance<T>(), null);
            }
            else
            {
                if (response.StatusCode ==
                    System.Net.HttpStatusCode.Unauthorized)
                {
                    string errorMessage = "Unauthorized access";
                    logger.LogWarning(errorMessage);
                    return (default, errorMessage);
                }

                var responseString
                    = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseString))
                {
                    return (default, responseString);
                }

                string generalErrorMessage
                    = $"Error sending HTTP request: {response.StatusCode}";
                logger.LogError(generalErrorMessage);
                return (default, generalErrorMessage);
            }
        }
        catch (HttpRequestException ex)
        {
            string errorMessage = $"HTTP request error: {ex.Message}";
            logger.LogError(ex, errorMessage);
            return (default, errorMessage);
        }
        catch (JsonException ex)
        {
            string errorMessage = $"JSON error: {ex.Message}";
            logger.LogError(ex, errorMessage);
            return (default, errorMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Error: {ex.Message}";
            logger.LogError(ex, errorMessage);
            return (default, errorMessage);
        }
    }

    private async Task<HttpResponseMessage> PostRequest(string uri,
        HttpContent content)
    {
        var url = BaseUrl + uri;
        url = AddAuthorizationHeader(url);
        try
        {
            var result = await httpClient.PostAsync(url, content);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(
                $"Error sending HTTP request: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }

    private string AddAuthorizationHeader(string uri)
    {
        // do not need token, because api require key
        // var token = Preferences.Get("Token", string.Empty);
        const string token = AppConfig.ApiKey;
        if (uri.Contains('?'))
        {
            uri += $"&APPID={token}";
        }
        else
        {
            uri += $"?APPID={token}";
        }

        uri += "&units=metric"; // Use metric units(Celsius)

        return uri;
    }
}
