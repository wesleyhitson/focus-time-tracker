using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace FocusTimeTracker.Converters;

public class StringNotNullOrEmptyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        return !string.IsNullOrWhiteSpace(value as string);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}

public class SelectedButtonConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is int selectedDays && parameter is string paramDays && int.TryParse(paramDays, out int buttonDays))
        {
            return selectedDays == buttonDays ? Brush.Parse("#7C3AED") : Brush.Parse("#3D3D4D");
        }
        return Brush.Parse("#3D3D4D");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}

public class MinutesToProgressConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is int minutes)
        {
            // Normalize to a max of 120 minutes for display
            return Math.Min(minutes / 120.0, 1.0);
        }
        return 0.0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}
