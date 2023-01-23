using DesktopFox.Enums;
using DesktopFox.MVVM.Model;
using DesktopFox.MVVM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Windows.Media;

namespace DesktopFox
{
    public class Wallpaper : ObserverNotifyChange
    {
        public Monitor myMonitor;

        public string myMediaUri;

        public VLCRotation myRotation = VLCRotation.None;

        public FPS myFPS = FPS.Min;
   
        private Volume _volume = Volume.Mute;
        public Volume Volume { get { return _volume; } set { _volume = value; RaisePropertyChanged(nameof(Volume)); } }
     

        [JsonIgnore]
        public IntPtr myHandler;

    }
}
