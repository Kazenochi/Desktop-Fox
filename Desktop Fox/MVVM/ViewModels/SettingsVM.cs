﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopFox.MVVM.ViewModels
{
    internal class SettingsVM : INotifyPropertyChanged
    {
        private Settings settings;
        public SettingsVM(Settings settings)
        {
            this.settings = settings;
        }

        private Boolean _settingsVisible = false;
        public Boolean SettingsVisible { get { return _settingsVisible; } set { _settingsVisible = value; RaisePropertyChanged(nameof(SettingsVisible)); } }

        public ICommand ToggleSettingsCommand { get { return new DF_Command.DelegateCommand(o => this.SettingsVisible = !this.SettingsVisible); } }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
