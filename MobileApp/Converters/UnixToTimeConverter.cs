using System.Globalization;

namespace MobileApp.Converters;

public class UnixToTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        if (value is int unixTime)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime)
                .LocalDateTime;
            return
                dateTime.ToString(
                    "hh:mm tt"); // Convert to local time in AM/PM format
        }

        return "N/A";
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
