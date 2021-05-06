using System;
using System.Globalization;
using System.Windows.Data;

namespace AnalyzerControlGUI.Converters
{
    [ValueConversion(typeof(bool), typeof(int))]
    public class BoolToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value == true) ? 0 : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value == 0) ? true : false;
        }
    }
}
