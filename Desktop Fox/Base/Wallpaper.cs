using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DesktopFox
{
    public class Wallpaper
    {
        public Monitor myMonitor;

        public string MediaUri;

        public Stretch myStretch;

        public IntPtr myHandler;

        public AnimatedWallpaperVM myViewModel;

        public AnimatedWallpaperModel myModel;
    }
}
