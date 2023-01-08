﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.Model
{
    public class Settings_InfoModel :ObserverNotifyChange
    {
        public string AppName { get { return _appName; } set { _appName = value; RaisePropertyChanged(nameof(AppName)); } } 
        private string _appName = System.AppDomain.CurrentDomain.FriendlyName;

        public string AppVersion { get { return _appVersion; } set { _appVersion = value; RaisePropertyChanged(nameof(AppVersion)); } }
        public string _appVersion { get; set;} = "Beta 0.1";

        public string FrameworkVersion { get { return _frameworkVersion; } set { _frameworkVersion = value; RaisePropertyChanged(nameof(FrameworkVersion)); } }
        public string _frameworkVersion { get; set; } = Environment.Version.ToString();

        public string Developer { get { return _developer; } set { _developer = value; RaisePropertyChanged(nameof(Developer)); } }
        public string _developer { get; set; } = "Andreas Keimel";

        public string Licence { get { return _licence; } set { _licence = value; RaisePropertyChanged(nameof(Licence)); } }
        public string _licence { get; set; } = "MIT"; 
    }
}
