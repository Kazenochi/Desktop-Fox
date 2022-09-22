using IDesktopWallpaperWrapper.Win32;
using System;
using System.Windows.Media;

namespace DesktopFox
{
    public class Settings
    {
        /// <summary>
        /// Zeitspannen für die umstellung zwischen dem Tag und Nacht Set, sowie der Zeit zwischen den Bildwechseln
        /// </summary>        
        public TimeSpan DayStart = new TimeSpan(8, 0, 0);               //Beginn des Tageszeitraums Bsp: 08:00 Uhr
        public TimeSpan NightStart = new TimeSpan(20, 0, 0);             //Beginn des Nachtzeitraums Bsp: 20:00 Uhr
        public TimeSpan ShuffleTime = new TimeSpan(0, 5, 0);

        /// <summary>
        /// Gibt den Zeitpunkt an ab dem ein neuer Tag angebrochen ist. 
        /// Bezieht sich auf das Ende der Programm Tageszeit nicht auf auf den Tatsächlichen Datumswechsel
        /// </summary>
        public DateTime NextDaySwitch = new DateTime();

        /// <summary>
        /// Sollen die Bilder Shuffeln
        /// </summary>
        public Boolean Shuffle = true;

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
    }
}