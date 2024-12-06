using System.Globalization;

namespace MobileApp.Converters;

public class MetersToKmConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        if (value is int meters)
        {
            return (meters / 1000.0).ToString("0.0"); // Convert to kilometers
        }

        return "N/A";
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
