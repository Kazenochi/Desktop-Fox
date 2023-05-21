using DesktopFox;
using IDesktopWallpaperWrapper.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using DPos = IDesktopWallpaperWrapper.Win32.DESKTOP_WALLPAPER_POSITION;

namespace DesktopFox
{
    /// <summary>
    /// Helferklasse für den Zugriff auf spezielle Einstellungen oder Änderungen.
    /// </summary>
    public class SettingsManager
    {
        private readonly Settings _settings;
        private GalleryManager GM;
        private readonly VirtualDesktop vDesk;
        private readonly Fox DF;
        private readonly Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="settings">Instanz der Einstellungen</param>
        /// <param name="virtualDesktop">Instanz des Virtuellen Hintergrundes</param>
        public SettingsManager(Fox DesktopFox, Settings settings, VirtualDesktop virtualDesktop)
        {
            DF = DesktopFox;
            _settings = settings;
            vDesk = virtualDesktop;
            SettingsCheckOnLoad();
            _settings.PropertyChanged += Settings_PropertyChanged;
        }

        /// <summary>
        /// Listener für Änderungen in den Einstellungen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_settings.DesktopFillMode):
                    vDesk.getWrapper.SetPosition((DESKTOP_WALLPAPER_POSITION)_settings.DesktopFillMode);
                    break;

                case nameof(_settings.ShuffleTime):
                    switch (_settings.ShuffleTime.TotalMinutes)
                    {
                        case 901:
                            _settings.OverrideShuffleTime(TimeSpan.FromSeconds(1));
                            break;
                        case 902:
                            _settings.OverrideShuffleTime(TimeSpan.FromSeconds(2));
                            break;
                        case 905:
                            _settings.OverrideShuffleTime(TimeSpan.FromSeconds(5));
                            break;
                        case 910:
                            _settings.OverrideShuffleTime(TimeSpan.FromSeconds(10));
                            break;
                        case 920:
                            _settings.OverrideShuffleTime(TimeSpan.FromSeconds(20));
                            break;
                        case 930:
                            _settings.OverrideShuffleTime(TimeSpan.FromSeconds(30));
                            break;
                    }
                    //Spezielles Anstoßen aufgrund der Zusatzfunktion 
                    DF.Shuffler.PicShuffleStart();
                    break;

                case nameof(_settings.DesktopModeSingle):
                    if (GM == null)
                        GM = DF.GalleryManager;

                    if (_settings.DesktopModeSingle && GM.getActiveSet(1) == null)
                        _settings.IsRunning = false;

                    else if (!_settings.DesktopModeSingle && GM.areSetsActive())
                        _settings.IsRunning = true;    
                    
                    break;

                case nameof(_settings.AutostartOn):
                    if (_settings.AutostartOn && regKey.GetValue("Desktopfox") == null)
                    {
                        string appLocation = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                        Debug.WriteLine("Autostart wurde gesetzt. Rückgabewert der App Location: " + appLocation);
                        regKey.SetValue("Desktopfox", appLocation);
                    }
                    else if (!_settings.AutostartOn && regKey.GetValue("Desktopfox") != null)
                    {
                        Debug.WriteLine("Registryeintrag wurde Entfernt");
                        regKey.DeleteValue("Desktopfox");
                    }
                    break;

                case nameof(_settings.DayStart):
                    if(_settings.NextDaySwitch.TimeOfDay != _settings.DayStart)
                        _settings.NextDaySwitch = _settings.NextDaySwitch.Subtract(_settings.NextDaySwitch.TimeOfDay).Add(_settings.DayStart);
                    break;

                default: return;
            }
        }

        /// <summary>
        /// Methode die Systemeinstellungen mit den gespeicherten Einstellungen abgleich
        /// Nach jetzigem Stand nur für den RegKey Eintrag notwendig
        /// </summary>
        private void SettingsCheckOnLoad()
        {
            if (_settings == null) return;

            if (_settings.AutostartOn && regKey.GetValue("Desktopfox") == null)
            {
                string appLocation = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                Debug.WriteLine("Autostart wurde gesetzt. Rückgabewert der App Location: " + appLocation);
                regKey.SetValue("Desktopfox", appLocation);
            }
            else if (!_settings.AutostartOn && regKey.GetValue("Desktopfox") != null)
            {
                Debug.WriteLine("Registryeintrag wurde Entfernt");
                regKey.DeleteValue("Desktopfox");
            }
        }

        /// <summary>
        /// Gibt das Einstellungsobject zurück
        /// </summary>
        public Settings Settings { get { return _settings; } }

        /// <summary>
        /// Gibt den Anzeigemodus des Desktops zurück der in den Einstellungen festgelegt ist
        /// </summary>
        /// <returns></returns>
        public DPos getDesktopFillMode()
        {
            return (DPos)_settings.DesktopFillMode;
        }
    }
}