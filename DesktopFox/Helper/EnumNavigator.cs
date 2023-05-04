using System;


namespace DesktopFox
{
    public static class EnumNavigator
    {

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
