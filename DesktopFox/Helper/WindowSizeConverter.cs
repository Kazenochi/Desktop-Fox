using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopFox
{
    /// <summary>
    /// Konverter für die Fenstergröße. Parameter gibt Skalierungsfaktor an
    /// </summary>
    internal class WindowSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            return size.ToString("G0", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
