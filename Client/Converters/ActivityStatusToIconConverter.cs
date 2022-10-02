using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.Converters
{
    public class ActivityStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ActivityStatus activityStatus = (ActivityStatus)value;
            switch (activityStatus)
            {
                case ActivityStatus.Online:
                    return PackIconKind.TickCircleOutline;
                case ActivityStatus.Away:
                    return PackIconKind.ClockTimeFourOutline;
                case ActivityStatus.DoNotDisturb:
                    return PackIconKind.MinusCircleOutline;
                default:
                    return PackIconKind.CloseCircleOutline;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
