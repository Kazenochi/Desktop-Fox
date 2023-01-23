

using DesktopFox.MVVM.Views;
using System.Windows.Controls;

namespace DesktopFox.MVVM.Model
{
    public class AnimatedWallpaperConfigModel : ObserverNotifyChange
    {

        private string _sourceUri = "";
        public string SourceUri { get { return _sourceUri; } set { _sourceUri = value; RaisePropertyChanged(nameof(SourceUri)); } }

        public bool[] _monitorVisibility = new bool[3];
        public bool Monitor1_Visible { get { return _monitorVisibility[0]; } set { _monitorVisibility[0] = value; RaisePropertyChanged(nameof(Monitor1_Visible)); } }
        public bool Monitor2_Visible { get { return _monitorVisibility[1]; } set { _monitorVisibility[1] = value; RaisePropertyChanged(nameof(Monitor2_Visible)); } }
        public bool Monitor3_Visible { get { return _monitorVisibility[2]; } set { _monitorVisibility[2] = value; RaisePropertyChanged(nameof(Monitor3_Visible)); } }

        public bool[] _monitors = new bool[3];
        public bool Monitor1 { get { return _monitors[0]; } set { _monitors[0] = value; RaisePropertyChanged(nameof(Monitor1)); } }
        public bool Monitor2 { get { return _monitors[1]; } set { _monitors[1] = value; RaisePropertyChanged(nameof(Monitor2)); } }
        public bool Monitor3 { get { return _monitors[2]; } set { _monitors[2] = value; RaisePropertyChanged(nameof(Monitor3)); } }

        public AnimatedWallpaperView[] _monitorViews = new AnimatedWallpaperView[3];
        public AnimatedWallpaperView Monitor1_Video { get { return _monitorViews[0]; } set { _monitorViews[0] = value; RaisePropertyChanged(nameof(Monitor1_Video)); } }
        public AnimatedWallpaperView Monitor2_Video { get { return _monitorViews[1]; } set { _monitorViews[1] = value; RaisePropertyChanged(nameof(Monitor2_Video)); } }
        public AnimatedWallpaperView Monitor3_Video { get { return _monitorViews[2]; } set { _monitorViews[2] = value; RaisePropertyChanged(nameof(Monitor3_Video)); } }

        private int _rotation = 0;
        public int Rotation { get { return _rotation; } set { _rotation = value; RaisePropertyChanged(nameof(Rotation)); } }

        private int _volume = 0;
        public int Volume { get { return _volume;} set { _volume = value; RaisePropertyChanged(nameof(Volume)); } }


    }
}
