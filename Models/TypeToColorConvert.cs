using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace StoreApp.Models
{
    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string type)
            {
                return type switch
                {
                    "Customer" => new SolidColorBrush(Colors.Green),
                    "Store" => new SolidColorBrush(Colors.Blue),
                    _ => new SolidColorBrush(Colors.Gray),
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
