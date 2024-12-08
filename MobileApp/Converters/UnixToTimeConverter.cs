using System.Globalization;

namespace MobileApp.Converters;

public class UnixToTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
            int unixTime;
            if (value is long unixTimeLong)
            {
                unixTime = (int)unixTimeLong;
            }
            else if (value is int unixTimeInt)
            {
                unixTime = unixTimeInt;
            }
            else if (value is string unixTimeString)
            {
                unixTime = int.Parse(unixTimeString);
            }
            else
            {
                return "N/A";
            }

            var dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime)
                .UtcDateTime; // Convert Unix timestamp to UTC DateTime
            return
                dateTime.ToString(
                    "hh:mm tt"); // Convert to local time in AM/PM format
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
