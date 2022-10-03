using DesktopFox;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using DPos = IDesktopWallpaperWrapper.Win32.DESKTOP_WALLPAPER_POSITION;

namespace DesktopFox
{
    public class SettingsManager
    {
        private Settings _settings;
        private GalleryManager GM;
        public SettingsManager(Settings settings)
        {
            this._settings = settings;
        }

        public void reInit(GalleryManager galleryManager)
        {
            GM = galleryManager;
        }

        private MainWindow _mainWindow;
        private Shuffler _shuffler;
        private Virtual_Desktop _virtualDesktop;
        Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public SettingsManager(MainWindow mainWindow,Shuffler shuffler, Virtual_Desktop virtual_Desktop, GalleryManager galleryManager)
        {
            _mainWindow = mainWindow;
            _shuffler = shuffler;
            _virtualDesktop = virtual_Desktop;
            GM = galleryManager;
        }

       

        /// <summary>
        /// Gibt das Einstellungsobject zurück
        /// </summary>
        public Settings getSettings()
        {
            return _settings;
        }

        /// <summary>
        /// Legt den Anzeigemodus für die Großen Preview Bilder fest
        /// </summary>
        private void BigPreview()
        {
         //   _mainWindow.image_PreviewBig.Stretch = _settings.PreviewFillMode;
          //  _mainWindow.image_PreviewBig_Fade.Stretch = _settings.PreviewFillMode;
        }

        /// <summary>
        /// Speichert die Einstellungen in einer JSON Datei
        /// </summary>
        public void save()
        {
          //  DF_Json.saveFile(_settings);
        }


        /// <summary>
        /// Setzt den Anzeigemodus für den Desktop 
        /// </summary>
        /// <param name="dPos"></param>
        public void setDesktopFillMode(DPos dPos)
        {
            _settings.DesktopFillMode = (int)dPos;
            _virtualDesktop.getWrapper.SetPosition(dPos);
        }

        /// <summary>
        /// Gibt den Anzeigemodus des Desktops zurück der in den Einstellungen festgelegt ist
        /// </summary>
        /// <returns></returns>
        public DPos getDesktopFillMode()
        {
            return (DPos)_settings.DesktopFillMode;
        }

        /// <summary>
        /// Soll der Desktop geshuffelt oder linear angezeigt werden. true = shuffeln
        /// </summary>
        /// <returns></returns>
        public bool getShuffle()
        {
            return _settings.Shuffle;

        }


        /// <summary>
        /// Setzt die Zeit des Shuffel Intervalls für den Desktop
        /// </summary>
        /// <param name="nwShuffleTime"></param>
        public void setShuffleTime(TimeSpan nwShuffleTime)
        {
            #region Gimick Funktionen 2 sekunden aufwärts funktioniert im multimodus und im Single 10 sekunden
            if (nwShuffleTime == TimeSpan.FromMinutes(901))
                nwShuffleTime = TimeSpan.FromSeconds(1);

            if (nwShuffleTime == TimeSpan.FromMinutes(902))
                nwShuffleTime = TimeSpan.FromSeconds(2);

            if (nwShuffleTime == TimeSpan.FromMinutes(905))
                nwShuffleTime = TimeSpan.FromSeconds(5);

            if (nwShuffleTime == TimeSpan.FromMinutes(910))
                nwShuffleTime = TimeSpan.FromSeconds(10);

            if (nwShuffleTime == TimeSpan.FromMinutes(920))
                nwShuffleTime = TimeSpan.FromSeconds(20);

            if (nwShuffleTime == TimeSpan.FromMinutes(930))
                nwShuffleTime = TimeSpan.FromSeconds(30);
            #endregion

            _settings.ShuffleTime = nwShuffleTime;
            if (_settings.IsRunning == true)
                _shuffler.picShuffleStart();
        }

        /// <summary>
        /// Gibt die Zeitspanne zurück die für den Desktop Shuffel eingestellt ist
        /// </summary>
        /// <returns></returns>
        public TimeSpan getShuffleTime()
        {
            return _settings.ShuffleTime;
        }

        /// <summary>
        /// Setzt das Flag das angibt ob das Programm den Desktop Managed
        /// </summary>
        /// <param name="state"></param>
        public void setRunning(bool state)
        {
            _settings.IsRunning = state;
        }

        /// <summary>
        /// Gibt zurück ob das Programm den Desktophintergrund Managed
        /// </summary>
        /// <returns></returns>
        public bool isRunning()
        {
            return _settings.IsRunning;
        }

        /// <summary>
        /// EVTL. Obsolete: 
        /// Ersetzt die Einstellungen komplett mit einem neuen object
        /// </summary>
        /// <param name="nwSettings"></param>
        public void changedSettings(Settings nwSettings)
        {
            _settings = nwSettings;
            BigPreview();
        }

        /// <summary>
        /// Gibt den eingestellten Beginn des Tages zurück
        /// </summary>
        /// <returns></returns>
        public TimeSpan getDayStart()
        {
            return _settings.DayStart;
        }

        /// <summary>
        /// Gibt den eingestellten Beginn der Nacht zurück
        /// </summary>
        /// <returns></returns>
        public TimeSpan getNightStart()
        {
            return _settings.NightStart;
        }

        /// <summary>
        /// Gibt den eingestellten Modus des Desktops zurück. Windows Shuffler <-> DF Shuffler
        /// </summary>
        /// <returns>WinShuffle = true, DFShuffle = false</returns>
        public bool getDesktopMode()
        {
            return _settings.DesktopModeSingle;
        }
     
        /// <summary>
        /// Setzt den Desktop Modus. 
        /// </summary>
        /// <param name="nwMode"></param>
        public void setDesktopMode(bool nwMode)
        {

         //if(_shuffler.isRunningDesktopTimer())
         if (isRunning())
             _shuffler.picShuffleStart();
      
        }

        /// <summary>
        /// Gibt den Zeitpunkt zurück an dem der Tageswechsel erfolgen soll
        /// </summary>
        /// <returns></returns>
        public DateTime getNextDaySwitch()
        {
            return _settings.NextDaySwitch;
        }

        /// <summary>
        /// Legt den Nächsten Tageswechsel Zeitpunkt fest
        /// </summary>
        /// <param name="dateTime"></param>
        public void setNextDaySwitch(DateTime dateTime)
        {
            _settings.NextDaySwitch = dateTime;
        }

        /// <summary>
        /// Setzt ob das Programm im Autostart sein soll
        /// </summary>
        /// <param name="state"></param>
        public void setAutostart(bool state)
        {
            if (state && regKey.GetValue("Desktopfox") == null)
            {
                string appLocation = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                //string appLocation = Assembly.GetExecutingAssembly().Location;
                Debug.WriteLine("Autostart wurde gesetzt. Rückgabewert der Applocation: " + appLocation);
                regKey.SetValue("Desktopfox", appLocation);
            }
            else if (state == false && regKey.GetValue("Desktopfox") != null)
            {
                Debug.WriteLine("Registryeintrag wurde Entfernt");
                regKey.DeleteValue("Desktopfox");
            }

            _settings.AutostartOn = state;
        }

        /// <summary>
        /// Gibt zurück ob das Programm im Autostart ist
        /// </summary>
        /// <returns></returns>
        public bool getAutostart()
        {
            return _settings.AutostartOn;
        }

        /// <summary>
        /// Setzt den Flag ob beim Tageswechsel das Aktive Set Automatisch gewechselt werden soll
        /// </summary>
        /// <param name="value">true = Automatischer Wechsel, false = Kein Wechsel</param>
        public void setAutoSetChange(bool value)
        {
            _settings.AutoSetChange = value;
            Debug.WriteLine("Auto SetChange wurde geändert, ist jetzt: " + value);
        }

        /// <summary>
        /// Gibt zurück ob beim Tageswechsel das Aktive Set Automatisch gewechselt werden soll
        /// </summary>
        /// <returns></returns>
        public bool getAutoSetChange()
        {
            return _settings.AutoSetChange;
        }

        }

    }