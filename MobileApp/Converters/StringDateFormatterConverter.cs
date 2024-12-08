using System.Globalization;

namespace MobileApp.Converters;

public class StringDateFormatterConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string date)
        {
            if (DateTime.TryParse(date, out DateTime result))
            {
                var temp = result.ToString("ddd dd MMM HH tt");
                return $"Date: {temp}";
            }
        }
        return value;
    }

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {
        return ((IValueConverter)this).ConvertBack(value, targetType, parameter, culture);
    }
}
