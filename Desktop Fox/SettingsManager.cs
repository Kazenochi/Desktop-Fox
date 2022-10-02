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

        /*
        private Settings _settings;
        //private Settings_MainView _settingsView;
        private MainWindow _mainWindow;
        private Shuffler _shuffler;
        private Virtual_Desktop _virtualDesktop;
        private GalleryManager GM;
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
        /// Setzt den Anzeigemodus für die Preview Bilder
        /// </summary>
        /// <param name="stretch"></param>
        public void setPreviewFillMode(Stretch stretch)
        {
            _settings.PreviewFillMode = stretch;
            BigPreview();
        }

        /// <summary>
        /// Setzt den Anzeigemodus für den Desktop 
        /// </summary>
        /// <param name="dPos"></param>
        public void setDesktopFillMode(DPos dPos)
        {
            _settings.DesktopFillMode = dPos;
            _virtualDesktop.getWrapper.SetPosition(dPos);
        }

        /// <summary>
        /// Gibt den Anzeigemodus des Desktops zurück der in den Einstellungen festgelegt ist
        /// </summary>
        /// <returns></returns>
        public DPos getDesktopFillMode()
        {
            return _settings.DesktopFillMode;
        }

        /// <summary>
        /// Setzt ob der Desktop geshuffelt werden soll
        /// </summary>
        /// <param name="state"></param>
        public void setShuffle(bool state)
        {
            _settings.Shuffle = state;
            if (_settings.isRunning) ;
              //  _mainWindow.shuffler.picShuffleStart();
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
            if (_settings.isRunning == true)
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
            _settings.isRunning = state;
        }

        /// <summary>
        /// Gibt zurück ob das Programm den Desktophintergrund Managed
        /// </summary>
        /// <returns></returns>
        public bool isRunning()
        {
            return _settings.isRunning;
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
        /// Setzt den Wert, wann der Tag beginnen soll
        /// </summary>
        /// <param name="nwDayStart"></param>
        public void setDayStart(TimeSpan nwDayStart, bool wait = false)
        {
            _settings.DayStart = nwDayStart;
            if (wait == false)
                _shuffler.daytimeTimerStart();
        }

        /// <summary>
        /// Setzt den Wert, wann die Nacht beginnen soll
        /// </summary>
        /// <param name="nwNightStart"></param>
        public void setNightStart(TimeSpan nwNightStart, bool wait = false)
        {
            _settings.NightStart = nwNightStart;
            if (wait == false)
                _shuffler.daytimeTimerStart();
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
        /// <returns></returns>
        public String getDesktopMode()
        {
            return _settings.DesktopMode;
        }
     
        /// <summary>
        /// Setzt den Desktop Modus. 
        /// Gültige Werte: "Single" "Multi"
        /// </summary>
        /// <param name="nwMode"></param>
        public void setDesktopMode(String nwMode)
        {
            /*
         _settings.DesktopMode = nwMode;
         if (nwMode == "Single")
         {
             if (_settings.isRunning && GM.getActiveSet() == null && GM.getActiveSet(any: true) != null)
             {
                 GM.setActiveSet(GM.getActiveSet(any: true));
             }

             foreach (PictureView i in _mainWindow.listBoxPreview.Items)
             {
                 i.pActiveMarker2.Visibility = System.Windows.Visibility.Collapsed;
                 i.pActiveMarker3.Visibility = System.Windows.Visibility.Collapsed;
             }
             _mainWindow.button_ActiveSet.Visibility = System.Windows.Visibility.Collapsed;
         }
         else if (nwMode == "Multi")
         {
             foreach (PictureView i in _mainWindow.listBoxPreview.Items)
             {
                 i.MarkerCheck();
                 _mainWindow.button_ActiveSet.Visibility = System.Windows.Visibility.Visible;
             }
             if (GM.getActiveSet(any: true) != null)
             {
                 _shuffler.picShuffleStart();
             }
         }
         else
         {
             Debug.WriteLine("Fehler bei der Zuweisung des DesktopModus in: " + nameof(setDesktopMode));
         }


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

            _settings.autostartOn = state;
        }

        /// <summary>
        /// Gibt zurück ob das Programm im Autostart ist
        /// </summary>
        /// <returns></returns>
        public bool getAutostart()
        {
            return _settings.autostartOn;
        }

        /// <summary>
        /// Setzt den Flag ob beim Tageswechsel das Aktive Set Automatisch gewechselt werden soll
        /// </summary>
        /// <param name="value">true = Automatischer Wechsel, false = Kein Wechsel</param>
        public void setAutoSetChange(bool value)
        {
            _settings.autoSetChange = value;
            Debug.WriteLine("Auto SetChange wurde geändert, ist jetzt: " + value);
        }

        /// <summary>
        /// Gibt zurück ob beim Tageswechsel das Aktive Set Automatisch gewechselt werden soll
        /// </summary>
        /// <returns></returns>
        public bool getAutoSetChange()
        {
            return _settings.autoSetChange;
        }

        */
        }

    }