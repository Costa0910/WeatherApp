using Microsoft.Extensions.Logging;
using MobileApp.Services;
using MobileApp.ViewModels;

namespace MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Add services
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<FavoriteService>();
        builder.Services.AddSingleton<CurrentWeatherViewModel>();
        builder.Services.AddSingleton<ForecastViewModel>();
        builder.Services.AddSingleton<CityWeatherViewModel>();

        return builder.Build();
    }
}
