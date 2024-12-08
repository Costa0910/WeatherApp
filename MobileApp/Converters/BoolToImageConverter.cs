using System.Globalization;

namespace MobileApp.Converters;

public class BoolToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        if (value is bool booleanValue)
        {
            return booleanValue
                ? "favorite_filled_icon.svg"
                : "favorite_icon.svg";
        }

        return "cross.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
