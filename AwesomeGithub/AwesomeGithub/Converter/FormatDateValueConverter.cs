using System;
using System.Globalization;
using Xamarin.Forms;

namespace AwesomeGithub.Converter
{
    public class FormatDateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTime)
            {
                var date = (DateTime)value;
                return date.ToShortDateString();
            }

            return DateTime.MinValue.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
