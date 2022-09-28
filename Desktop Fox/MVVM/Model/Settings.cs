using IDesktopWallpaperWrapper.Win32;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace DesktopFox
{
    public class Settings : ObserverNotifyChange
    {
        /// <summary>
        /// Zeitspannen für die umstellung zwischen dem Tag und Nacht Set, sowie der Zeit zwischen den Bildwechseln
        /// </summary>        
        private TimeSpan _dayStart = new TimeSpan(8, 0, 0);               //Beginn des Tageszeitraums Bsp: 08:00 Uhr
        public TimeSpan DayStart { get { return _dayStart; } set { _dayStart = value; RaisePropertyChanged(nameof(DayStart)); } }
        
        private TimeSpan _nightStart = new TimeSpan(20, 0, 0);             //Beginn des Nachtzeitraums Bsp: 20:00 Uhr
        public TimeSpan NightStart { get { return _nightStart; } set { _nightStart = value; RaisePropertyChanged(nameof(NightStart)); } }

        private TimeSpan _shuffleTime = new TimeSpan(0, 5, 0);
        public TimeSpan ShufflerTime { get { return _shuffleTime; } set { _shuffleTime = value; RaisePropertyChanged(nameof(ShufflerTime)); } }

        /// <summary>
        /// Gibt den Zeitpunkt an ab dem ein neuer Tag angebrochen ist. 
        /// Bezieht sich auf das Ende der Programm Tageszeit nicht auf auf den Tatsächlichen Datumswechsel
        /// </summary>
        private DateTime _nextDaySwitch = new DateTime();
        public DateTime NextDaySwitch { get { return _nextDaySwitch; } set { _nextDaySwitch = value; RaisePropertyChanged(nameof(NextDaySwitch)); } } 


        /// <summary>
        /// Sollen die Bilder Shuffeln
        /// </summary>
        private Boolean _shuffle = true;
        public Boolean Shuffle { get { return _shuffle; } set { _shuffle = value; RaisePropertyChanged(nameof(Shuffle)); } }


        /// <summary>
        /// Wie das Preview Bild in der Anwendung angezeigt werden soll
        /// Gültige Werte sind: "Fill" & "Stretch" & "UniFill"
        /// </summary>
        private Stretch _previewFillMode = Stretch.Fill;
        public Stretch PreviewFillMode { get { return _previewFillMode; } set { _previewFillMode = value; RaisePropertyChanged(nameof(PreviewFillMode)); } }


        //Note: Tile macht meines erachtens wenig sinn für die meiste art von Hintergründen (Niemand mag Tile -_- )
        /// <summary>
        /// Wie sollen die Hintergründe auf dem Desktop angezeigt werden.
        ///DWPOS_TILE    = Tiles the image across all Monitors
        ///DWPOS_SPAN    = Spans a single image across all Monitors
        ///DWPOS_CENTER  = Centers the image. No strech
        ///DWPOS_FILL    = Stretches image across the screen and cropps as neccesary to avoid letterboxes
        ///DWPOS_FIT     = Scales image to monitor size without changing aspect ratio. Can lead to letterboxes.
        /// </summary>
        private DESKTOP_WALLPAPER_POSITION _desktopFillMode = DESKTOP_WALLPAPER_POSITION.DWPOS_FILL;
        public DESKTOP_WALLPAPER_POSITION DesktopFillMode { get { return _desktopFillMode; } set { _desktopFillMode = value; RaisePropertyChanged(nameof(DesktopFillMode)); } }


        /// <summary>
        /// Marker ob das Programm aktuell den Desktop Managed. Wird beim Start abgefragt um die nötigen Timer zu starten
        /// </summary>
        private Boolean _isRunning = false;
        public Boolean IsRunning { get { return _isRunning; } set { _isRunning = value; RaisePropertyChanged(nameof(IsRunning)); } }


        /// <summary>
        /// Flag ob das Programm im Autostart ausgeführt wird
        /// </summary>
        private Boolean _autostartOn = false;
        public Boolean AutostartOn { get { return _autostartOn; } set { _autostartOn = value; RaisePropertyChanged(nameof(AutostartOn)); } }


        /// <summary>
        /// Flag ob beim Tageswechsel die Aktiven Sets gewechselt werden sollen
        /// </summary>
        private Boolean _autoSetChange = false;
        public Boolean AutoSetChange { get { return _autoSetChange; } set { _autoSetChange = value; RaisePropertyChanged(nameof(AutoSetChange)); } }


        /// <summary>
        /// Einstellung welcher Shuffler verwendet werden soll, eigener oder Windows
        /// true = Single = Zeigt ein Set auf beiden Monitoren an
        /// false = Multi = Zeigt unterschiedliche Sets pro Monitor an
        /// </summary>
        private Boolean _desktopModeSingle = true;
        public Boolean DesktopModeSingle { get { return _desktopModeSingle; } set { _desktopModeSingle = value; RaisePropertyChanged(nameof(DesktopModeSingle)); } }
    }
}