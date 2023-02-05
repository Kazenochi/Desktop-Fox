using System;


namespace DesktopFox
{
    /// <summary>
    /// Helferklasse für den Tageswechsel Check
    /// </summary>
    public static class CheckDaySwitch
    {
        /// <summary>
        /// Ermitteln ob ein Tageswechsel durchgeführt werden muss und ermittlung des nächsten Tageswechsels
        /// </summary>
        /// <param name="now">Aktuelle Zeit von der Ausgegangen werden soll. Sollte Default <see cref="DateTime.Now"/> sein</param>
        /// <param name="nextSwitch">Tageswechselzeit die geprüft werden soll</param>
        /// <returns></returns>
        public static DateTime Check(DateTime now, DateTime nextSwitch)
        {
            if (now < nextSwitch && Math.Abs((nextSwitch - now).Days) < 1) return nextSwitch;

            int AddDay = 0;
            if(now.TimeOfDay > nextSwitch.TimeOfDay) 
                AddDay = 1;

            nextSwitch = now.Date.Add(TimeSpan.FromDays(AddDay).Add(nextSwitch.TimeOfDay));
            return nextSwitch;
        }
    }
}
