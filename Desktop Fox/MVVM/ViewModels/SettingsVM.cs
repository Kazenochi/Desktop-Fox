using CommunityToolkit.Mvvm.ComponentModel;
using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.Views;
using IDesktopWallpaperWrapper.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopFox.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel der <see cref="Views.Settings_MainView"/> Klasse
    /// </summary>
    public class SettingsVM : ObserverNotifyChange
    {
        public Settings settings { get; set; }
        public Settings_DaytimeModel DaytimeModel { get; set; } = new Settings_DaytimeModel();
        public Settings_ShuffleModel ShuffleModel { get; set; } = new Settings_ShuffleModel();
        public Settings_InfoModel InfoModel { get; set; } = new Settings_InfoModel();

        private Settings_DaytimeView _daytimeView = new Settings_DaytimeView();
        private Settings_DModeView _dmodeView = new Settings_DModeView();
        private Settings_PreviewView _previewView = new Settings_PreviewView();
        private Settings_ShuffleView _shuffleView = new Settings_ShuffleView();
        private Settings_StyleView _styleView = new Settings_StyleView();
        private Settings_SystemView _systemView = new Settings_SystemView();
        private Settings_LanguageView _languageView = new Settings_LanguageView();
        private Settings_Info _info = new Settings_Info();

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="settings"></param>
        public SettingsVM(Settings settings)
        {
            this.settings = settings;
            _daytimeView.DataContext = this;
            _dmodeView.DataContext = this;
            _previewView.DataContext = this;
            _shuffleView.DataContext = this;
            _styleView.DataContext = this;
            _systemView.DataContext = this;
            _languageView.DataContext = this;
            _info.DataContext = this;

            CurrentView = _previewView; 
        }

        /// <summary>
        /// Anzeige der aktuellen Settings View in <see cref="Settings.SettingsPage"/>
        /// </summary>
        public object CurrentView { get { return _currentView; } set { _currentView = value; UpdateNumbers(); RaisePropertyChanged(nameof(CurrentView)); } }
        private object _currentView;

        public int Language { get { return _language; } set { _language = value; LanguageChange(); RaisePropertyChanged(nameof(Language)); } }
        private int _language = 0;
       
        private void LanguageChange()
        {
            switch (Language)
            {
                case 0:
                    break;
                case 1:
                    break;
            }
        }

        /// <summary>
        /// Aktualisiert die Zahlenwerte in den Einsetllungs Views
        /// </summary>
        private void UpdateNumbers()
        {
            DaytimeModel.SetDaySwitch(settings.DayStart, settings.NightStart);
            ShuffleModel.SetShuffle(settings.ShuffleTime);
        }

        /// <summary>
        /// Speichert die Zeitspanne in den Temporären Einstellungs Views, in die Einstellungen
        /// </summary>
        private void SaveDaytimeValues()
        {
            List<TimeSpan> tmpValues = DaytimeModel.SaveValues();
            settings.DayStart = tmpValues[0];
            settings.NightStart = tmpValues[1];
        }

        /// <summary>
        /// Speichert die Zeitspanne in den Temporären Einstellungs Views, in die Einstellungen
        /// </summary>
        private void SaveShuffleTimeValue()
        {
            settings.ShuffleTime = TimeSpan.FromMinutes(ShuffleModel.ShuffleTime);
        }

        #region Kommandos
        /// <summary>
        /// Kommando um die <see cref="Views.Settings_DaytimeView"/> anzuzeigen
        /// </summary>
        public ICommand DaytimeCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _daytimeView); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_DModeView"/> anzuzeigen
        /// </summary>
        public ICommand DModeCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _dmodeView); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_PreviewView"/> anzuzeigen
        /// </summary>
        public ICommand PreviewCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _previewView); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_ShuffleView"/> anzuzeigen
        /// </summary>
        public ICommand ShuffleCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _shuffleView); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_StyleView"/> anzuzeigen
        /// </summary>
        public ICommand StyleCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _styleView); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_SystemView"/> anzuzeigen
        /// </summary>
        public ICommand SystemCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _systemView); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_SystemView"/> anzuzeigen
        /// </summary>
        public ICommand LanguageCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _systemView); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_Info"/> anzuzeigen
        /// </summary>
        public ICommand InfoCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _info); } }

        /// <summary>
        /// Kommando um die temporären Tageszeitwechsel Einstellungen abzuspeichern
        /// </summary>
        public ICommand SaveDaytimeCommand { get { return new DF_Command.DelegateCommand(o => SaveDaytimeValues()); } }

        /// <summary>
        /// Kommando um die temporären Shuffle Zeit Einstellungen abzuspeichern
        /// </summary>
        public ICommand SaveShuffleTimeCommand { get { return new DF_Command.DelegateCommand(o => SaveShuffleTimeValue()); } }

        public ICommand DaytimeTextChangedCommand { get { return new DF_Command.DelegateCommand(o => SaveDaytimeValues()); } }
        #endregion

    }
}
