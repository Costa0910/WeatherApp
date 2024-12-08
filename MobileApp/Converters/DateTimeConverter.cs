using System.Globalization;

namespace MobileApp.Converters;

public class DateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        long timestamp = 0;
        long timezoneOffset = 0;
        if (value is int intValue)
        {
            timestamp = intValue;
        }
        else if (value is long longValue)
        {
            timestamp = longValue;
        }

        if (parameter is int intParameter)
        {
            timezoneOffset = intParameter;
        }
        else if (parameter is long longParameter)
        {
            timezoneOffset = longParameter;
        }

        if (timestamp != 0 && timezoneOffset != 0)
        {
            // Convert Unix timestamp to UTC DateTime
            DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp)
                .UtcDateTime;

            // Adjust for the timezone offset
            DateTime localDateTime = utcDateTime.AddSeconds(timezoneOffset);

            return localDateTime.ToString("ddd dd MMM HH");
        }

        return DateTime.Now.ToString("ddd dd MMM HH tt");
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
    {
        // ConvertBack is not needed in this case
        throw new NotImplementedException();
    }
}
