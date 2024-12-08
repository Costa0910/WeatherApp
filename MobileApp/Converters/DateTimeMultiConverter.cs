using System;
using System.Globalization;

namespace MobileApp.Converters;

public class DateTimeMultiConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter,
        CultureInfo culture)
    {
        if (values[0] is int timestamp && values[1] is int timezoneOffset)
        {
            // Convert Unix timestamp to UTC DateTime
            DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp)
                .UtcDateTime;

            // Adjust for the timezone offset
            DateTime localDateTime = utcDateTime.AddSeconds(timezoneOffset);

            return localDateTime.ToString("ddd dd MMM HH:mm tt");
        }

        return "N/A";
    }

    public object[] ConvertBack(object value, Type[] targetTypes,
        object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
