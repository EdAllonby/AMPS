using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Client.View.Converter
{
    internal sealed class BoolToIconConverter : IValueConverter
    {
        /// <summary>
        /// Coverts a bool into a static resource icon.
        /// </summary>
        /// <param name="value">The boolean to convert.</param>
        /// <param name="targetType">No specified target type.</param>
        /// <param name="parameter">No parameter.</param>
        /// <param name="culture">No culture.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return Application.Current.Resources["CheckmarkIcon"];
            }

            return Application.Current.Resources["CloseIcon"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}