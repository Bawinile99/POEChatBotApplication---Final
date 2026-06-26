using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace POEChatBotApplication.Converters
{
    public class BoolToBubbleColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If true (user message), use purple; else (bot message), use dark gray.
            return (bool)value
                ? new SolidColorBrush(Color.FromRgb(0x80, 0x00, 0x80))
                : new SolidColorBrush(Color.FromRgb(0x30, 0x30, 0x30));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
