using LibVLCSharp.Shared;
using Newtonsoft.Json;
using System;


namespace DesktopFox
{
    public class Wallpaper : ObserverNotifyChange
    {
        public Monitor myMonitor;

        public string myMediaUri;

        public VLCRotation myRotation = VLCRotation.None;

        public FPS myFPS = FPS.Min;
   
        private VLCVolume _volume = VLCVolume.Mute;
        public VLCVolume Volume { get { return _volume; } set { _volume = value; RaisePropertyChanged(nameof(Volume)); } }

        private VLCState _playPause = VLCState.Playing;
        public VLCState PlayPause { get { return _playPause; } set { _playPause = value; RaisePropertyChanged(nameof(PlayPause)); } }

        [JsonIgnore]
        public IntPtr myHandler;

    }
}
