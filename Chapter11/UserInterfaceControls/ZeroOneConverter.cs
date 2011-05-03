using System;
using System.Globalization;
using System.Windows.Data;

namespace UserInterfaceControls
{
    public class ZeroOneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            if (s != null)
            {
                s = s.ToLower();
                if (s == "on")
                    return "1";
                else if (s == "off")
                    return "0";
                else
                    return value;
            }

            bool? b = value as bool?;
            if (b.HasValue)
            {
                return b == true ? "1" : "0";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
