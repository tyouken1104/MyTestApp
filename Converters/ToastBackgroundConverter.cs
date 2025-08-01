using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Globalization;

namespace MyTestApp.Converters
{
    public class ToastBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return new SolidColorBrush(Colors.Transparent);
            
            string colorString = value.ToString() ?? "#0078D4";
            
            // 获取当前主题 - 简化为直接返回亮色主题颜色
            // 主题检测将在应用层处理
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

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ToastBackgroundDarkConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return new SolidColorBrush(Colors.Transparent);
            
            string colorString = value.ToString() ?? "#0078D4";
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

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}