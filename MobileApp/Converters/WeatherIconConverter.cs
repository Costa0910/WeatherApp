using System.Globalization;

namespace MobileApp.Converters;

public class WeatherIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string iconCode)
        {
            return $"https://openweathermap.org/img/wn/{iconCode}@2x.png";
        }
        return "default_weather_icon.png"; // Placeholder image
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
