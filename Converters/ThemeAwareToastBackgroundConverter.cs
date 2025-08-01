using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Globalization;

namespace MyTestApp.Converters
{
    public class ThemeAwareToastBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return new SolidColorBrush(Colors.Transparent);
            
            string colorString = value.ToString() ?? "#0078D4";
            
            // 获取当前主题
            var app = Avalonia.Application.Current;
            if (app != null && app.ActualThemeVariant == ThemeVariant.Dark)
            {
                // 暗色主题颜色
                switch (colorString)
                {
                    case "#107C10": // Success
                        return new SolidColorBrush(Color.Parse("#0A3410"));
                    case "#FF8C00": // Warning
                        return new SolidColorBrush(Color.Parse("#4A2B00"));
                    case "#D13438": // Error
                        return new SolidColorBrush(Color.Parse("#4A0F0F"));
                    case "#0078D4": // Info
                        return new SolidColorBrush(Color.Parse("#0A2A4A"));
                    default:
                        return new SolidColorBrush(Color.Parse("#0A2A4A"));
                }
            }
            else
            {
                // 亮色主题颜色
                switch (colorString)
                {
                    case "#107C10": // Success
                        return new SolidColorBrush(Color.Parse("#E8F5E8"));
                    case "#FF8C00": // Warning
                        return new SolidColorBrush(Color.Parse("#FFF4E8"));
                    case "#D13438": // Error
                        return new SolidColorBrush(Color.Parse("#FEE7E7"));
                    case "#0078D4": // Info
                        return new SolidColorBrush(Color.Parse("#E8F4FD"));
                    default:
                        return new SolidColorBrush(Color.Parse("#E8F4FD"));
                }
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}