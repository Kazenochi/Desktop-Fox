using System;

namespace DesktopFox.MVVM.Model
{
    /// <summary>
    /// Model für die Shuffel Zeit Einstellungs <see cref="ViewModels.SettingsVM"/> Klasse
    /// </summary>
    public class Settings_ShuffleModel : ObserverNotifyChange
    {
        /// <summary>
        /// Zeit in Minuten die Zwischen dem Bildwechsel liegen soll, der in der UI angezeigt werden soll <see cref="Views.Settings_ShuffleView.shuffleTime"/>
        /// </summary>
        public int ShuffleTime { get { return _shuffleTime; } set { _shuffleTime = value; RaisePropertyChanged(nameof(_shuffleTime)); } }
        private int _shuffleTime = 0;            //Beginn des Nachtzeitraums Bsp: 20:00 Uhr

        /// <summary>
        /// umwandeln der Minuten in integer wert
        /// </summary>
        /// <param name="shuffleTime"></param>
        public void SetShuffle(TimeSpan shuffleTime)
        {
            ShuffleTime = shuffleTime.Minutes;
        }
    }
}
