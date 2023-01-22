using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Windows.Media;

namespace DesktopFox
{
    public class Wallpaper
    {
        public Monitor myMonitor;

        public string MediaUri;

        public Stretch myStretch;

        public int myRotation;

        public bool muted;

        [JsonIgnore]
        public IntPtr myHandler;

        [JsonIgnore]
        public AnimatedWallpaperVM myViewModel;

        [JsonIgnore]
        public AnimatedWallpaperModel myModel;
    }
}
