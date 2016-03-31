using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EOkno.Views
{
    public class NegateBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var val = (bool)value;
                return (val) ? Visibility.Collapsed : Visibility.Visible;
            }
            catch
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
