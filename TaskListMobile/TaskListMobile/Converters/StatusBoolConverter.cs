using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TaskListMobileData.Enums;
using Xamarin.Forms;

namespace TaskListMobile.Converters
{
    public class StatusBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TaskItemStatus)value) == TaskItemStatus.Completed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return TaskItemStatus.Completed;
            }
            else
            {
                return TaskItemStatus.Pending;
            }
        }
    }
}
