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
    public class SettingsVM : ObserverNotifyChange
    {
        public Settings settings { get; set; }
        public Settings_DaytimeModel DaytimeModel { get; set; } = new Settings_DaytimeModel();
        public Settings_ShuffleModel ShuffleModel { get; set; } = new Settings_ShuffleModel();

        private Settings_DaytimeView _daytimeView = new Settings_DaytimeView();
        private Settings_DModeView _dmodeView = new Settings_DModeView();
        private Settings_PreviewView _previewView = new Settings_PreviewView();
        private Settings_ShuffleView _shuffleView = new Settings_ShuffleView();
        private Settings_StyleView _styleView = new Settings_StyleView();
        
        public SettingsVM(Settings settings)
        {
            this.settings = settings;
            _daytimeView.DataContext = this;
            _dmodeView.DataContext = this;
            _previewView.DataContext = this;
            _shuffleView.DataContext = this;
            _styleView.DataContext = this;

            CurrentView = _previewView; 
        }


        private object _currentView;
        public object CurrentView { get { return _currentView; } set { _currentView = value; UpdateNumbers(); RaisePropertyChanged(nameof(CurrentView)); } }

        private void UpdateNumbers()
        {
            DaytimeModel.SetDaySwitch(settings.DayStart, settings.NightStart);
            ShuffleModel.SetShuffle(settings.ShufflerTime);
        }

        private void SaveDaytimeValues()
        {
            List<TimeSpan> tmpValues = DaytimeModel.SaveValues();
            settings.DayStart = tmpValues[0];
            settings.NightStart = tmpValues[1];
        }

        private void SaveShuffleTimeValue()
        {
            settings.ShufflerTime = TimeSpan.FromMinutes(ShuffleModel.ShuffleTime);
        }

        public ICommand DaytimeCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _daytimeView); } }
        public ICommand DModeCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _dmodeView); } }
        public ICommand PreviewCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _previewView); } }
        public ICommand ShuffleCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _shuffleView); } }
        public ICommand StyleCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _styleView); } }
        public ICommand SaveDaytimeCommand { get { return new DF_Command.DelegateCommand(o => SaveDaytimeValues()); } }
        public ICommand SaveShuffleTimeCommand { get { return new DF_Command.DelegateCommand(o => SaveShuffleTimeValue()); } }
        public ICommand DaytimeTextChangedCommand { get { return new DF_Command.DelegateCommand(o => SaveDaytimeValues()); } }
    }
}
