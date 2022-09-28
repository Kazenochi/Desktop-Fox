using CommunityToolkit.Mvvm.ComponentModel;
using DesktopFox.MVVM.Views;
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

        private Boolean _settingsVisible = false;
        public Boolean SettingsVisible { get { return _settingsVisible; } set { _settingsVisible = value; RaisePropertyChanged(nameof(SettingsVisible)); } }

        private object _currentView;
        public object CurrentView { get { return _currentView; } set { _currentView = value; RaisePropertyChanged(nameof(CurrentView)); } }

        public ICommand ToggleSettingsCommand { get { return new DF_Command.DelegateCommand(o => this.SettingsVisible = !this.SettingsVisible); } }
        public ICommand DaytimeCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _daytimeView); } }
        public ICommand DModeCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _dmodeView); } }
        public ICommand PreviewCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _previewView); } }
        public ICommand ShuffleCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _shuffleView); } }
        public ICommand StyleCommand { get { return new DF_Command.DelegateCommand(o => CurrentView = _styleView); } }
    }
}
