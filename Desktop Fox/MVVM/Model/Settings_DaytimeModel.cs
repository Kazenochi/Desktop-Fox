using System;
using System.Collections.Generic;


namespace DesktopFox.MVVM.Model
{
    /// <summary>
    /// Model für die Tageszeiteinstellungen in der <see cref="ViewModels.SettingsVM"/> Klasse
    /// </summary>
    public class Settings_DaytimeModel : ObserverNotifyChange
    {
        #region Binding Variablen
        /// <summary>
        /// Beginn der Tageszeit. Stunden Wert in der UI <see cref="Views.Settings_DaytimeView.dayHours"/>
        /// </summary>
        public int DayStartHours { get { return _dayStartHours; } set { _dayStartHours = value; RaisePropertyChanged(nameof(DayStartHours)); } }
        private int _dayStartHours = 8;               //Beginn des Tageszeitraums Bsp: 08:00 Uhr

        /// <summary>
        /// Beginn der Nachtzeit. Stunden Wert in der UI <see cref="Views.Settings_DaytimeView.nightHours"/>
        /// </summary>
        public int NightStartHours { get { return _nightStartHours; } set { _nightStartHours = value; RaisePropertyChanged(nameof(NightStartHours)); } }
        private int _nightStartHours = 20;      //Beginn des Nachtzeitraums Bsp: 20:00 Uhr

        /// <summary>
        /// Beginn der Tageszeit. Minuten Wert in der UI <see cref="Views.Settings_DaytimeView.dayMinutes"/>
        /// </summary>
        public int DayStartMinutes { get { return _dayStartMinutes; } set { _dayStartMinutes = value; RaisePropertyChanged(nameof(DayStartMinutes)); } }
        private int _dayStartMinutes = 0;               //Beginn des Tageszeitraums Bsp: 08:00 Uhr

        /// <summary>
        /// Beginn der Nachtzeit. Minuten Wert in der UI <see cref="Views.Settings_DaytimeView.nightMinutes"/>
        /// </summary>
        public int NightStartMinutes { get { return _nightStartMinutes; } set { _nightStartMinutes = value; RaisePropertyChanged(nameof(NightStartMinutes)); } }
        private int _nightStartMinutes = 0;            //Beginn des Nachtzeitraums Bsp: 20:00 Uhr
        #endregion

        #region Helfer Methoden
        /// <summary>
        /// Helferfunktion um die Zeitpunkte von Tag und Nacht Beginn in den Einstellungen, in Stunden und Minuten aufzuteilen.
        /// </summary>
        /// <param name="dayStart"><see cref="Settings.DayStart"/></param>
        /// <param name="nightStart"><see cref="Settings.NightStart"/></param>
        public void SetDaySwitch(TimeSpan dayStart, TimeSpan nightStart)
        {
            DayStartHours = dayStart.Hours;
            NightStartHours = nightStart.Hours;
            DayStartMinutes = dayStart.Minutes;
            NightStartMinutes = nightStart.Minutes;
        }
        
        /// <summary>
        /// Helferfunktion um die Aufgeteilte Zeit wieder in eine Zeitspanne für die Einstellungen umzuwandeln
        /// </summary>
        /// <returns>[0]<see cref="Settings.DayStart"/> [1]<see cref="Settings.NightStart"/></returns>
        public List<TimeSpan> SaveValues()
        {
            TimeSpan dayStart = new TimeSpan();
            TimeSpan nightStart = new TimeSpan();

            if (DayStartHours < 0 || (DayStartHours > 24 && DayStartMinutes > 0))
                DayStartHours = 8;
            if(DayStartMinutes < 0 || DayStartMinutes > 59)
                DayStartMinutes = 0;
            if(NightStartHours < 0 || (NightStartHours > 24 && NightStartMinutes > 0))
                NightStartHours = 20;
            if(NightStartMinutes < 0 || NightStartMinutes > 59)
                NightStartMinutes = 0;

            dayStart = TimeSpan.FromHours(DayStartHours) + TimeSpan.FromMinutes(DayStartMinutes);
            nightStart = TimeSpan.FromHours(NightStartHours) + TimeSpan.FromMinutes(NightStartMinutes);

            return new List<TimeSpan> { dayStart, nightStart };
        }
        #endregion
    }
}
