using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DesktopFox.MVVM.Model
{
    class BackgroundWindowModel :ObserverNotifyChange
    {
        private int _height;
        public int Height { get { return _height; } set { _height = value; RaisePropertyChanged(nameof(Height)); } }

        private int _width;
        public int Width { get { return _width;} set { _width = value;RaisePropertyChanged(nameof(Width)); } }

        private int _monitor;
        public int Monitor { get { return _monitor;} set { _monitor = value;RaisePropertyChanged(nameof(Monitor)); } }

        private UserControl _wallpaper;
        public UserControl Wallpaper { get { return _wallpaper; } set { _wallpaper = value; RaisePropertyChanged(nameof(Wallpaper)); } }
    }
}
