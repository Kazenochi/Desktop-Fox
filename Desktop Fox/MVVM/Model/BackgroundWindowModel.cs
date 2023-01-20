using DesktopFox.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DesktopFox.MVVM.Model
{
    public class BackgroundWindowModel :ObserverNotifyChange
    {
        private AnimatedWallpaperView _wallpaper;
        public AnimatedWallpaperView Wallpaper { get { return _wallpaper; } set { _wallpaper = value; RaisePropertyChanged(nameof(Wallpaper)); } }
    }
}
