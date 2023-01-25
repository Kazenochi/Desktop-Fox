using DesktopFox.MVVM.Views;
using System.Windows.Controls;

namespace DesktopFox.MVVM.Model
{
    public class AnimatedWallpaperConfigModel : ObserverNotifyChange
    {

        /// <summary>
        /// Quell Uri für VLC Videos. Zwischenspeicher bevor die Videos mit <see cref="AnimatedWallpaperView"/> erstellt werden werden
        /// </summary>
        public string SourceUri { get { return _sourceUri; } set { _sourceUri = value; RaisePropertyChanged(nameof(SourceUri)); } }
        private string _sourceUri = "";

        /// <summary>
        /// Toggle Variable für die Anzeige der Play/Pause Buttons in <see cref="AnimatedWallpaperConfigView"/>
        /// </summary>
        public bool PlayPauseToggle { get { return _playPauseToggle; } set { _playPauseToggle = value; RaisePropertyChanged(nameof(PlayPauseToggle)); } }
        private bool _playPauseToggle = false;

        #region Visibility Binding der Monitor Toggle Buttons in Config View
        public bool[] _monitorVisibility = new bool[3];
        public bool Monitor1_Visible { get { return _monitorVisibility[0]; } set { _monitorVisibility[0] = value; RaisePropertyChanged(nameof(Monitor1_Visible)); } }
        public bool Monitor2_Visible { get { return _monitorVisibility[1]; } set { _monitorVisibility[1] = value; RaisePropertyChanged(nameof(Monitor2_Visible)); } }
        public bool Monitor3_Visible { get { return _monitorVisibility[2]; } set { _monitorVisibility[2] = value; RaisePropertyChanged(nameof(Monitor3_Visible)); } }
        #endregion

        #region Toggle Button "isChecked" Binding. Entscheided ob Video Angezeigt werden soll
        public bool[] _monitors = new bool[3];
        public bool Monitor1 { get { return _monitors[0]; } set { _monitors[0] = value; Video1Visibility = value; RaisePropertyChanged(nameof(Monitor1)); } }
        public bool Monitor2 { get { return _monitors[1]; } set { _monitors[1] = value; Video2Visibility = value; RaisePropertyChanged(nameof(Monitor2)); } }
        public bool Monitor3 { get { return _monitors[2]; } set { _monitors[2] = value; Video3Visibility = value; RaisePropertyChanged(nameof(Monitor3)); } }
        #endregion

        #region VLC Views zur Anzeige in Config Fenster
        public AnimatedWallpaperView[] _monitorVideos = new AnimatedWallpaperView[3];
        public AnimatedWallpaperView Monitor1_Video { get { return _monitorVideos[0]; } set { _monitorVideos[0] = value; RaisePropertyChanged(nameof(Monitor1_Video)); } }
        public AnimatedWallpaperView Monitor2_Video { get { return _monitorVideos[1]; } set { _monitorVideos[1] = value; RaisePropertyChanged(nameof(Monitor2_Video)); } }
        public AnimatedWallpaperView Monitor3_Video { get { return _monitorVideos[2]; } set { _monitorVideos[2] = value; RaisePropertyChanged(nameof(Monitor3_Video)); } }
        #endregion

        #region Visibility Binding der VLC Videos im Config Fenster 
        public bool[] _videoVisibility = new bool[3];
        public bool Video1Visibility { get { return _videoVisibility[0]; } set { _videoVisibility[0] = value; RaisePropertyChanged(nameof(Video1Visibility)); } }
        public bool Video2Visibility { get { return _videoVisibility[1]; } set { _videoVisibility[1] = value; RaisePropertyChanged(nameof(Video2Visibility)); } }
        public bool Video3Visibility { get { return _videoVisibility[2]; } set { _videoVisibility[2] = value; RaisePropertyChanged(nameof(Video3Visibility)); } }
        #endregion

        /// <summary>
        /// Backupvariable um Volume Zwischen zu Speichern
        /// </summary>
        public VLCVolume Volume { get { return _volume; } set { _volume = value; VolumeProgress = (int)value; } }
        private VLCVolume _volume = VLCVolume.Mute;

        /// <summary>
        /// Lautstärke Anzeige als Progressbar
        /// </summary>
        public int VolumeProgress { get { return _volumeProgress; } set { _volumeProgress = value; RaisePropertyChanged(nameof(VolumeProgress)); } }
        private int _volumeProgress;

        /// <summary>
        /// Binding Variable ob das VLC Hintergrundvideo aktuell gemutet ist
        /// </summary>
        public bool IsMuted { get { return _isMuted; } set { _isMuted = value; RaisePropertyChanged(nameof(IsMuted)); } }
        private bool _isMuted = false;
    }
}
