using IDesktopWallpaperWrapper.Win32;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace DesktopFox
{
    public class Settings : ObserverNotifyChange
    {
        /// <summary>
        /// Tageszeitpunkt an dem der Tag beginnt. Zeit wird als Zeitspanne ab 00:00 Uhr angegeben.
        /// </summary>        
        public TimeSpan DayStart { get { return _dayStart; } set { _dayStart = value; RaisePropertyChanged(nameof(DayStart)); } }
        private TimeSpan _dayStart = new TimeSpan(8, 0, 0);               //Beginn des Tageszeitraums Bsp: 08:00 Uhr

        /// <summary>
        /// Tageszeitpunkt an dem die Nacht beginnt. Zeit wird als Zeitspanne ab 00:00 Uhr angegeben.
        /// </summary>   
        public TimeSpan NightStart { get { return _nightStart; } set { _nightStart = value; RaisePropertyChanged(nameof(NightStart)); } }
        private TimeSpan _nightStart = new TimeSpan(20, 0, 0);             //Beginn des Nachtzeitraums Bsp: 20:00 Uhr

        /// <summary>
        /// Zeit Intervall in dem das Hintergrundbild gewechsel wird.
        /// </summary>   
        public TimeSpan ShufflerTime { get { return _shuffleTime; } set { _shuffleTime = value; RaisePropertyChanged(nameof(ShufflerTime)); } }
        private TimeSpan _shuffleTime = new TimeSpan(0, 5, 0);


        /// <summary>
        /// Gibt den Zeitpunkt an ab dem ein neuer Tag angebrochen ist. 
        /// Bezieht sich auf das Ende der Programm Tageszeit nicht auf auf den Tatsächlichen Datumswechsel
        /// </summary>
        public DateTime NextDaySwitch { get { return _nextDaySwitch; } set { _nextDaySwitch = value; RaisePropertyChanged(nameof(NextDaySwitch)); } }
        private DateTime _nextDaySwitch = new DateTime();

        /// <summary>
        /// Sollen die Bilder Shuffeln
        /// </summary>
        public Boolean Shuffle { get { return _shuffle; } set { _shuffle = value; RaisePropertyChanged(nameof(Shuffle)); } }
        private Boolean _shuffle = true;

        /// <summary>
        /// Wie das Preview Bild in der Anwendung angezeigt werden soll
        /// Werte sind: "1 = Fill" & " 2 = Uniform/Stretch" & "3 = Uniform Fill"
        /// </summary>
        public int PreviewFillMode { get { return (int)_previewFillMode; } set { _previewFillMode = (Stretch)value; RaisePropertyChanged(nameof(PreviewFillMode)); } }
        private Stretch _previewFillMode = Stretch.Fill;

        //Note: Tile macht meines erachtens wenig sinn für die meiste art von Hintergründen (Niemand mag Tile -_- )
        /// <summary>
        /// Wie sollen die Hintergründe auf dem Desktop angezeigt werden.
        ///DWPOS_CENTER = 0 = Zentriertes Bild das Mittig auf dem Bildschirm dargestellt wird. Kein Strecken oder ändern des Seitenverhältnisses.
        ///DWPOS_TILE   = 1 = Splaziert das Bild in einem Ziegelmuster über alle Monitore.
        ///DWPOS_STRETCH= 2 = Streckt das Bild über den gesamten Monitor und ändert das Seitenverhältnis.
        ///DWPOS_FIT    = 3 = Scalliert das Bild auf die Monitorgröße. Seitenverhältnis wird behalten. Letterboxen können entstehen.
        ///DWPOS_FILL   = 4 = Skaliert das Bild bis der Monitor komplett gefüllt ist. Schneidet Teile des Bildes ab.
        ///DWPOS_SPAN   = 5 = Streckt ein Bild über alle Monitore 
        /// </summary>
        public int DesktopFillMode { get { return (int)_desktopFillMode; } set { _desktopFillMode = (DESKTOP_WALLPAPER_POSITION)value; RaisePropertyChanged(nameof(DesktopFillMode)); } }
        private DESKTOP_WALLPAPER_POSITION _desktopFillMode = DESKTOP_WALLPAPER_POSITION.DWPOS_FILL;

        /// <summary>
        /// Marker ob das Programm aktuell den Desktop Managed. Wird beim Start abgefragt um die nötigen Timer zu starten
        /// </summary>
        public Boolean IsRunning { get { return _isRunning; } set { _isRunning = value; RaisePropertyChanged(nameof(IsRunning)); } }
        private Boolean _isRunning = false;

        /// <summary>
        /// Flag ob das Programm im Autostart ausgeführt wird
        /// </summary>
        public Boolean AutostartOn { get { return _autostartOn; } set { _autostartOn = value; RaisePropertyChanged(nameof(AutostartOn)); } }
        private Boolean _autostartOn = false;

        /// <summary>
        /// Flag ob beim Tageswechsel die Aktiven Sets gewechselt werden sollen
        /// </summary>
        public Boolean AutoSetChange { get { return _autoSetChange; } set { _autoSetChange = value; RaisePropertyChanged(nameof(AutoSetChange)); } }
        private Boolean _autoSetChange = false;

        /// <summary>
        /// Einstellung welcher Shuffler verwendet werden soll, eigener oder Windows
        /// true = Single = Zeigt ein Set auf beiden Monitoren an
        /// false = Multi = Zeigt unterschiedliche Sets pro Monitor an
        /// </summary>
        public Boolean DesktopModeSingle { get { return _desktopModeSingle; } set { _desktopModeSingle = value; RaisePropertyChanged(nameof(DesktopModeSingle)); } }
        private Boolean _desktopModeSingle = true;
    }
}