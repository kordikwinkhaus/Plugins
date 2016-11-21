using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WindowOffset.Views
{
    public class CenterItemConverter : IMultiValueConverter
    {
        private static CenterItemConverter s_instance = new CenterItemConverter();
        public static CenterItemConverter Instance
        {
            get { return s_instance; }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double)values[0];
            double height = (double)values[1];

            double left = width / -2.0;
            double top = height / -2.0;

            return new Thickness(left, top, left, top);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
