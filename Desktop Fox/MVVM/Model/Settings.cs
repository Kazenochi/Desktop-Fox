using IDesktopWallpaperWrapper.Win32;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace DesktopFox
{
    public class Settings : INotifyPropertyChanged
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
        public Stretch PreviewFillMode = Stretch.Fill;

        //Note: Tile macht meines erachtens wenig sinn für die meiste art von Hintergründen (Niemand mag Tile -_- )
        /// <summary>
        /// Wie sollen die Hintergründe auf dem Desktop angezeigt werden.
        ///DWPOS_TILE    = Tiles the image across all Monitors
        ///DWPOS_SPAN    = Spans a single image across all Monitors
        ///DWPOS_CENTER  = Centers the image. No strech
        ///DWPOS_FILL    = Stretches image across the screen and cropps as neccesary to avoid letterboxes
        ///DWPOS_FIT     = Scales image to monitor size without changing aspect ratio. Can lead to letterboxes.
        /// </summary>
        public DESKTOP_WALLPAPER_POSITION DesktopFillMode = DESKTOP_WALLPAPER_POSITION.DWPOS_FILL;

        /// <summary>
        /// Marker ob das Programm aktuell den Desktop Managed. Wird beim Start abgefragt um die nötigen Timer zu starten
        /// </summary>
        public Boolean isRunning = false;

        /// <summary>
        /// Flag ob das Programm im Autostart ausgeführt wird
        /// </summary>
        public Boolean autostartOn = false;

        /// <summary>
        /// Flag ob beim Tageswechsel die Aktiven Sets gewechselt werden sollen
        /// </summary>
        public Boolean autoSetChange = false;

        /// <summary>
        /// Einstellung welcher Shuffler verwendet werden soll, eigener oder Windows
        /// Single  = Zeigt ein Set auf beiden Monitoren an
        /// Multi   = Zeigt unterschiedliche Sets pro Monitor an
        /// </summary>
        public String DesktopMode = "Single";






        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}