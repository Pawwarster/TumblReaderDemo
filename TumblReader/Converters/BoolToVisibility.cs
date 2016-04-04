using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TumblReader.Converters
{
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool))
                return Visibility.Collapsed;

            string paramStr = string.Empty;
            if (parameter is string)
                paramStr = parameter.ToString();

            switch (paramStr)
            {
                case "reverse":
                    return (bool)value ? Visibility.Collapsed : Visibility.Visible;
                default:
                    return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
