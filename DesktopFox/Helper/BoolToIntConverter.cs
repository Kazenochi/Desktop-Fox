using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopFox
{
    /// <summary>
    /// Helferklasse für die Repräsentation eines Integers als Boolean Wert
    /// </summary>
    public class BoolToIntConverter : IValueConverter
    {
        /// <summary>
        /// Vergleicht ob der Parameter mit dem Value Objekt übereinstimmt und gibt einen Boolean Wert zurück 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Bei Übereinstimmung wird ein true zurückgegeben andernfalls ein false</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int integer = (int)value;
            if (integer == int.Parse(parameter.ToString()))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
