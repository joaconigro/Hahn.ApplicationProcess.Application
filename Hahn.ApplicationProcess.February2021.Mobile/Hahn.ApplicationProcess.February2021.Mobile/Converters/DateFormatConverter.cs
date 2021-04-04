using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Hahn.Mobile.Converters
{
    /// <summary>
    /// This class have methods to convert the int to date format value. 
    /// </summary>
    [Preserve(AllMembers = true)]
    public class DateFormatConverter : IValueConverter
    {
        /// <summary>
        /// This method is used to convert the int to date format value.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the date format value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = ((DateTime)value).ToUniversalTime();
            return date.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// This method is used to convert the date format to int.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the int.</returns>     
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
