using System;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Globalization;

namespace PhoneMaps
{
    public class ItineraryItemTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string item = value as string;
            return Regex.Replace(item, "<.*?>", string.Empty);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
