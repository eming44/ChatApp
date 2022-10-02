using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Client.Converters
{
    public class ActivityStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ActivityStatus activityStatus = (ActivityStatus)value;
            return Regex.Replace(activityStatus.ToString(), "([a-z])([A-Z])", "$1 $2").Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
