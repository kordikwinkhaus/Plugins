using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WindowOffset.Views
{
    public class OffsetSideTextboxMarginConverter : IMultiValueConverter
    {
        private static OffsetSideTextboxMarginConverter s_instance = new OffsetSideTextboxMarginConverter();
        public static OffsetSideTextboxMarginConverter Instance
        {
            get { return s_instance; }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[2] == DependencyProperty.UnsetValue) return DependencyProperty.UnsetValue;
            if (values[3] == DependencyProperty.UnsetValue) return DependencyProperty.UnsetValue;

            double width = (double)values[0];
            double height = (double)values[1];
            double x = (double)values[2];
            double y = (double)values[3];

            double left = x - (width / 2.0);
            double top = y - (height / 2.0);

            return new Thickness(left, top, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
