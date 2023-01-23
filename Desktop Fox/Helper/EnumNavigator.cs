using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox
{
    public static class EnumNavigator
    {
        //https://stackoverflow.com/a/643438/20939585 von Yahya Hussein unter CC BY-SA 4.0
        //Erweitert mit Count Down Funktion

        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} ist kein Enum Wert", typeof(T).FullName));

            T[] tmpArray = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(tmpArray, src) + 1;
            return (tmpArray.Length == j) ? tmpArray[0] : tmpArray[j];
        }

        public static T Previous<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} ist kein Enum Wert", typeof(T).FullName));

            T[] tmpArray = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(tmpArray, src) - 1;
            return (0 > j) ? tmpArray[tmpArray.Length - 1] : tmpArray[j];
        }       
    }
}
