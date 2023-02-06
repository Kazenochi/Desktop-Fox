using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DesktopFox.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel der <see cref="Views.Settings_MainView"/> Klasse
    /// </summary>
    public class SettingsVM : ObserverNotifyChange
    {
        public Settings settings { get; set; }
        public Settings_DaytimeModel DaytimeModel { get; set; } = new();
        public Settings_ShuffleModel ShuffleModel { get; set; } = new();
        public Settings_InfoModel InfoModel { get; set; } = new();

        private Settings_DaytimeView _daytimeView = new();
        private Settings_DModeView _dmodeView = new();
        private Settings_PreviewView _previewView = new();
        private Settings_ShuffleView _shuffleView = new();
        private Settings_StyleView _styleView = new();
        private Settings_SystemView _systemView = new();
        private Settings_LanguageView _languageView = new();
        private Settings_Info _info = new();

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

        #region Binding Variablen

        /// <summary>
        /// Anzeige der aktuellen Settings View in <see cref="Settings.SettingsPage"/>
        /// </summary>
        public object CurrentView { get { return _currentView; } set { _currentView = value; UpdateNumbers(); RaisePropertyChanged(nameof(CurrentView)); } }
        private object _currentView;

        #endregion

        #region Kommandos

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_DaytimeView"/> anzuzeigen
        /// </summary>
        public ICommand DaytimeCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(_daytimeView)); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_DModeView"/> anzuzeigen
        /// </summary>
        public ICommand DModeCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(_dmodeView)); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_PreviewView"/> anzuzeigen
        /// </summary>
        public ICommand PreviewCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(_previewView)); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_ShuffleView"/> anzuzeigen
        /// </summary>
        public ICommand ShuffleCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(_shuffleView)); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_StyleView"/> anzuzeigen
        /// </summary>
        public ICommand StyleCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(_styleView)); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_SystemView"/> anzuzeigen
        /// </summary>
        public ICommand SystemCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(_systemView)); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_SystemView"/> anzuzeigen
        /// </summary>
        public ICommand LanguageCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(_systemView)); } }

        /// <summary>
        /// Kommando um die <see cref="Views.Settings_Info"/> anzuzeigen
        /// </summary>
        public ICommand InfoCommand { get { return new DF_Command.DelegateCommand(o => SwitchViews(_info)); } }

        /// <summary>
        /// Kommando um die temporären Tageszeitwechsel Einstellungen abzuspeichern
        /// </summary>
        public ICommand SaveDaytimeCommand { get { return new DF_Command.DelegateCommand(o => SaveDaytimeValues()); } }

        /// <summary>
        /// Kommando um die temporären Shuffle Zeit Einstellungen abzuspeichern
        /// </summary>
        public ICommand SaveShuffleTimeCommand { get { return new DF_Command.DelegateCommand(o => SaveShuffleTimeValue()); } }

        /* Note: Sehr warscheinliche alter verlassener Code
        /// <summary>
        /// Kommando um die temporären Shuffle Zeit Einstellungen abzuspeichern
        /// </summary>
        public ICommand DaytimeTextChangedCommand { get { return new DF_Command.DelegateCommand(o => SaveDaytimeValues()); } }
        */

        #endregion

        #region Methoden

        /// <summary>
        /// Ändernt die Views im Mainwindow <see cref="MainWindow.ContextViews"/>
        /// </summary>
        /// <param name="newView">Neue View die Angezeigt werden soll. null = keine View anzeigen.</param>
        public void SwitchViews(AnimatedBaseView newView)
        {
            if (newView != null && newView != CurrentView)
            {
                newView.FadeIn();
                CurrentView = newView;
            }
        }

        /// <summary>
        /// Aktualisiert die Zahlenwerte in den Einstellungs Views
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
            settings.ShuffleTime = ShuffleModel.SaveValues(); 
        }

        #endregion
    }
}
