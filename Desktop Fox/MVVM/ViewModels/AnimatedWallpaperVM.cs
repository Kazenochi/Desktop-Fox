using DesktopFox.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.MVVM.ViewModels
{
    public class AnimatedWallpaperVM
    {

        public AnimatedWallpaperModel AnimatedWallpaperModel { get; set; } = new();

        public AnimatedWallpaperVM(Wallpaper wallpaper)
        {
            this.AnimatedWallpaperModel.SourceUri = wallpaper.MediaUri;
        }

    }
}
