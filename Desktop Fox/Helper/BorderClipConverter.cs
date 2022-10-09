using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace DesktopFox
{
    /// <summary>
    /// ClipConverter von Marat Khasanov https://stackoverflow.com/a/5650367  Lizenz "CC BY-SA 3.0"
    /// Helferklasse um einen Clipping Layer dynamisch anzupassen
    /// </summary>
    public class BorderClipConverter : IMultiValueConverter
    {
        /// <summary>
        /// Erzeugt anhand der Dimensionen und des Eckradius eine neue Rechteckgeometrie die als Clipping Layer verwendet werden kann
        /// </summary>
        /// <param name="values">Object Array mit den Dimensionen und dem Eckradius. [0]=Breite, [1]=Höhe, [2]=Radius</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 && values[0] is double && values[1] is double && values[2] is CornerRadius)
            {
                var width = (double)values[0];
                var height = (double)values[1];

                if (width < Double.Epsilon || height < Double.Epsilon)
                {
                    return Geometry.Empty;
                }

                var radius = (CornerRadius)values[2];

                // Actually we need more complex geometry, when CornerRadius has different values.
                // But let me not to take this into account, and simplify example for a common value.
                var clip = new RectangleGeometry(new Rect(0, 0, width, height), radius.TopLeft, radius.TopLeft);
                clip.Freeze();

                return clip;
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
