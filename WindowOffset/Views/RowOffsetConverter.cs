using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WindowOffset.ViewModels;

namespace WindowOffset.Views
{
    public class RowOffsetConverter : IValueConverter
    {
        private static Thickness s_normal = new Thickness(4, 0, 0, 0);
        private static Thickness s_indent = new Thickness(20, 0, 0, 0);

        private static RowOffsetConverter s_instance = new RowOffsetConverter();
        public static RowOffsetConverter Instance
        {
            get { return s_instance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MainOffsetItemViewModel)
            {
                return s_normal;
            }
            else
            {
                return s_indent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
