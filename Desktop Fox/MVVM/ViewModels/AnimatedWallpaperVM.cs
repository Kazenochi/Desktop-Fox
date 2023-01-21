using DesktopFox.MVVM.Model;

namespace DesktopFox.MVVM.ViewModels
{
    public class AnimatedWallpaperVM
    {

        public AnimatedWallpaperModel AnimatedWallpaperModel { get; set; } = new();

        public AnimatedWallpaperVM(Wallpaper wallpaper)
        {
            this.AnimatedWallpaperModel.SourceUri = wallpaper.MediaUri;
            this.AnimatedWallpaperModel.Rotation = wallpaper.myRotation;
            this.AnimatedWallpaperModel.IsMuted = wallpaper.muted;
        }

    }
}
