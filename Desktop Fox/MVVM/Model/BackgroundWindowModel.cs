using DesktopFox.MVVM.Views;

namespace DesktopFox.MVVM.Model
{
    public class BackgroundWindowModel :ObserverNotifyChange
    {
        private AnimatedWallpaperView _wallpaper;
        public AnimatedWallpaperView Wallpaper { get { return _wallpaper; } set { _wallpaper = value; RaisePropertyChanged(nameof(Wallpaper)); } }
    }
}
