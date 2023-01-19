using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.Model
{
    class AnimatedWallpaperConfigModel : ObserverNotifyChange
    {
        private string _sourceUri = "D:\\VS\\Projekte\\Desktop Fox\\Desktop Fox\\Assets\\shenheXganyu.mp4";
        public string SourceUri { get { return _sourceUri; } set { _sourceUri = value; RaisePropertyChanged(nameof(SourceUri)); } }

        private bool _monitor1_Visible = false;
        public bool Monitor1_Visible { get { return _monitor1_Visible;} set { _monitor1_Visible = value; RaisePropertyChanged(nameof(Monitor1_Visible)); } }

        private bool _monitor2_Visible = false;
        public bool Monitor2_Visible { get { return _monitor2_Visible;} set { _monitor2_Visible = value; RaisePropertyChanged(nameof(Monitor2_Visible)); } }

        private bool _monitor3_Visible = false;
        public bool Monitor3_Visible { get { return _monitor3_Visible; } set { _monitor3_Visible = value; RaisePropertyChanged(nameof(Monitor3_Visible)); } }

        private bool _monitor1 = false;
        public bool Monitor1 { get { return _monitor1; } set { _monitor1 = value; RaisePropertyChanged(nameof(Monitor1)); } }

        private bool _monitor2 = false;
        public bool Monitor2 { get { return _monitor2; } set { _monitor2 = value; RaisePropertyChanged(nameof(Monitor2)); } }

        private bool _monitor3 = false;
        public bool Monitor3 { get { return _monitor3; } set { _monitor3 = value; RaisePropertyChanged(nameof(Monitor3)); } }

    }
}
