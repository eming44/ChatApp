using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.Converters
{
    public class ActivityStatusToColorConverter : IValueConverter
    {
        public SolidColorBrush OnlineStatusBrushColor { get; private set; } = (SolidColorBrush)new BrushConverter().ConvertFrom("#70b54e");

        public SolidColorBrush AwayStatusBrushColor { get; private set; } = (SolidColorBrush)new BrushConverter().ConvertFrom("#f59e1b");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            ActivityStatus activityStatus = (ActivityStatus)value;
            switch(activityStatus)
            {
                case ActivityStatus.Online:
                    return this.OnlineStatusBrushColor;
                case ActivityStatus.Away:
                    return this.AwayStatusBrushColor;
                case ActivityStatus.DoNotDisturb:
                    return Brushes.Red;
                default:
                    return Brushes.Gray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
