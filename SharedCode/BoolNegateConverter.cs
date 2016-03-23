using System;
using System.Globalization;
using System.Windows.Data;

namespace Okna.Plugins
{
    public class BoolNegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return !(bool)value;
            }
            catch
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return !(bool)value;
            }
            catch
            {
                return false;
            }
        }
    }
}
