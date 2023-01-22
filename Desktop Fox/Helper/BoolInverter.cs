using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopFox
{
    /// <summary>
    /// Boolean Invertierer Helfer Klasse 
    /// </summary>
    public class BoolInverter : IValueConverter
    {
        /// <summary>
        /// Invertiert den übergebenen Boolean Wert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        /// <summary>
        /// Invertiert den übergebenen Boolean Wert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }
    }
}
